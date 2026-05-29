import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';

export async function runSync() {
  const store = useStore.getState();
  if (!store.online || !store.user || store.syncing) return;

  store.setSyncing(true);
  console.log("Iniciando sincronização offline em segundo plano...");

  try {
    // 1. Sincronizar Ambientes (Unidade)
    const localAmbs = await db.ambientes.where('syncStatus').notEqual('synced').toArray();
    for (const amb of localAmbs) {
      if (amb.syncStatus === 'created') {
        try {
          const res = await api.post('/Unidade/Unidades', {
            nome: amb.nome,
            idUsuario: store.user.Id
          });
          const newRemoteId = res?.Id || res?.id;
          if (newRemoteId) {
            await db.ambientes.update(amb.id, {
              idRemote: newRemoteId,
              syncStatus: 'synced'
            });
            // Atualiza os itens e listas dependentes locais que apontavam para o ID temporário local
            await db.listas.where('unidadeId').equals(amb.id).modify({ unidadeId: newRemoteId });
            await db.itens.where('unidadeId').equals(amb.id).modify({ unidadeId: newRemoteId });
          }
        } catch (e) {
          console.error("Falha ao sincronizar ambiente criado:", e);
        }
      }
    }

    // 2. Sincronizar Mercados (Sessao)
    const localMercados = await db.mercados.where('syncStatus').notEqual('synced').toArray();
    for (const merc of localMercados) {
      if (merc.syncStatus === 'created') {
        try {
          const res = await api.post('/Sessao', { sNome: merc.nome });
          if (res && res.Id) {
            await db.mercados.update(merc.id, {
              idRemote: res.Id,
              syncStatus: 'synced'
            });
            // Atualiza setores e itens dependentes locais
            await db.setores.where('sessaoId').equals(merc.id).modify({ sessaoId: res.Id });
            await db.itens.where('sessaoId').equals(merc.id).modify({ sessaoId: res.Id });
          }
        } catch (e) {
          console.error("Falha ao sincronizar mercado criado:", e);
        }
      } else if (merc.syncStatus === 'updated' && merc.idRemote) {
        try {
          await api.put(`/Sessao/${merc.idRemote}`, {
            id: merc.idRemote,
            sNome: merc.nome
          });
          await db.mercados.update(merc.id, { syncStatus: 'synced' });
        } catch (e) {
          console.error("Falha ao sincronizar mercado atualizado:", e);
        }
      } else if (merc.syncStatus === 'deleted' && merc.idRemote) {
        try {
          await api.delete(`/Sessao/${merc.idRemote}`);
          await db.mercados.delete(merc.id);
        } catch (e) {
          console.error("Falha ao sincronizar mercado deletado:", e);
        }
      }
    }

    // 3. Sincronizar Setores (Setor)
    const localSetores = await db.setores.where('syncStatus').notEqual('synced').toArray();
    for (const set of localSetores) {
      // Obter o ID remoto da sessão correspondente
      let sessaoIdRemote = set.sessaoId;
      if (typeof set.sessaoId === 'number' && set.sessaoId < 100000) { // assumindo ID temporário do Dexie
        const merc = await db.mercados.get(set.sessaoId);
        sessaoIdRemote = merc?.idRemote || set.sessaoId;
      }

      if (set.syncStatus === 'created' && sessaoIdRemote) {
        try {
          const res = await api.post('/Setor', {
            sNome: set.nome,
            nIdSessao: sessaoIdRemote
          });
          if (res && res.Id) {
            await db.setores.update(set.id, {
              idRemote: res.Id,
              syncStatus: 'synced'
            });
            await db.itens.where('setorId').equals(set.id).modify({ setorId: res.Id });
          }
        } catch (e) {
          console.error("Falha ao sincronizar setor criado:", e);
        }
      } else if (set.syncStatus === 'updated' && set.idRemote && sessaoIdRemote) {
        try {
          await api.put(`/Setor/${set.idRemote}`, {
            id: set.idRemote,
            sNome: set.nome,
            nIdSessao: sessaoIdRemote
          });
          await db.setores.update(set.id, { syncStatus: 'synced' });
        } catch (e) {
          console.error("Falha ao sincronizar setor atualizado:", e);
        }
      } else if (set.syncStatus === 'deleted' && set.idRemote) {
        try {
          await api.delete(`/Setor/${set.idRemote}`);
          await db.setores.delete(set.id);
        } catch (e) {
          console.error("Falha ao sincronizar setor deletado:", e);
        }
      }
    }

    // 4. Sincronizar Itens do Catálogo (Item)
    const localItens = await db.itens.where('syncStatus').notEqual('synced').toArray();
    for (const item of localItens) {
      // Mapear dependências locais para IDs remotos
      const amb = await db.ambientes.get(item.unidadeId);
      const merc = await db.mercados.get(item.sessaoId);
      const set = item.setorId ? await db.setores.get(item.setorId) : null;

      const remoteUnidadeId = amb?.idRemote || item.unidadeId;
      const remoteSessaoId = merc?.idRemote || item.sessaoId;
      const remoteSetorId = set ? (set.idRemote || item.setorId) : null;

      if (item.syncStatus === 'created' && remoteUnidadeId && remoteSessaoId) {
        try {
          const res = await api.post('/Item', {
            sNome: item.nome,
            sUnidadeMedida: item.unidadeMedida || 'UN',
            nIdUnidade: remoteUnidadeId,
            nIdSessao: remoteSessaoId,
            nIdSetor: remoteSetorId
          });
          if (res && res.Id) {
            await db.itens.update(item.id, {
              idRemote: res.Id,
              syncStatus: 'synced'
            });
            // Atualiza itens na lista locais
            await db.itensLista.where('itemId').equals(item.id).modify({ itemIdRemote: res.Id });
          }
        } catch (e) {
          console.error("Falha ao sincronizar item criado:", e);
        }
      } else if (item.syncStatus === 'updated' && item.idRemote && remoteUnidadeId && remoteSessaoId) {
        try {
          await api.put(`/Item/${item.idRemote}`, {
            id: item.idRemote,
            sNome: item.nome,
            sUnidadeMedida: item.unidadeMedida || 'UN',
            nIdUnidade: remoteUnidadeId,
            nIdSessao: remoteSessaoId,
            nIdSetor: remoteSetorId
          });
          await db.itens.update(item.id, { syncStatus: 'synced' });
        } catch (e) {
          console.error("Falha ao sincronizar item atualizado:", e);
        }
      } else if (item.syncStatus === 'deleted' && item.idRemote) {
        try {
          await api.delete(`/Item/${item.idRemote}`);
          await db.itens.delete(item.id);
        } catch (e) {
          console.error("Falha ao sincronizar item deletado:", e);
        }
      }
    }

    // 5. Sincronizar Listas de Compras (Lista)
    const localListas = await db.listas.where('syncStatus').notEqual('synced').toArray();
    for (const list of localListas) {
      const amb = await db.ambientes.get(list.unidadeId);
      const remoteUnidadeId = amb?.idRemote || list.unidadeId;

      if (list.syncStatus === 'created' && remoteUnidadeId) {
        try {
          // POST /Lista/SalvarLista
          const res = await api.post('/Lista/SalvarLista', {
            nome: list.nome,
            usuarioId: store.user.Id,
            unidadeId: remoteUnidadeId,
            itensLista: []
          });
          
          // Na API, SetLista retorna um bool ou a própria lista.
          // Para obter o ID da lista criada, ideal recarregar a lista de compras
          await db.listas.update(list.id, { syncStatus: 'synced' });
        } catch (e) {
          console.error("Falha ao sincronizar lista criada:", e);
        }
      } else if (list.syncStatus === 'deleted' && list.idRemote) {
        try {
          await api.delete(`/Lista/Deletar/${list.idRemote}`);
          await db.listas.delete(list.id);
        } catch (e) {
          console.error("Falha ao sincronizar lista deletada:", e);
        }
      }
    }

    // 6. Sincronizar Itens das Listas (ItemLista)
    const localItensLista = await db.itensLista.where('syncStatus').notEqual('synced').toArray();
    
    // Agrupa alterações por Lista para fazer chamadas atômicas
    const listasIdsModificadas = [...new Set(localItensLista.map(i => i.listaId))];
    
    for (const lId of listasIdsModificadas) {
      const listaObj = await db.listas.get(lId);
      if (!listaObj || !listaObj.idRemote) continue; // aguarda lista sincronizar

      const itensDaListaModificados = localItensLista.filter(i => i.listaId === lId);
      
      const payloadItens = itensDaListaModificados.map(item => ({
        id: item.idRemote || null,
        itemId: item.itemIdRemote || null,
        quantidade: item.syncStatus === 'deleted' ? null : item.quantidade, // null indica exclusão no backend
        preco: item.preco,
        statusItemListaId: item.statusItemListaId
      }));

      try {
        const payload = {
          listaId: listaObj.idRemote,
          usuarioLogadoId: store.user.Id,
          itensLista: payloadItens
        };

        await api.post('/ItemLista/Atualizar', payload);

        // Atualiza status local
        for (const item of itensDaListaModificados) {
          if (item.syncStatus === 'deleted') {
            await db.itensLista.delete(item.id);
          } else {
            await db.itensLista.update(item.id, { syncStatus: 'synced' });
          }
        }
      } catch (e) {
        console.error(`Falha ao sincronizar itens da lista ${lId}:`, e);
      }
    }

    console.log("Sincronização offline em segundo plano concluída com sucesso.");
  } catch (err) {
    console.error("Erro geral na rotina de sincronização:", err);
  } finally {
    store.setSyncing(false);
  }
}
