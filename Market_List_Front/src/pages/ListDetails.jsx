import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { ArrowLeft, Plus, Minus, Trash2, Search, ShoppingBag } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function ListDetails() {
  const { id } = useParams(); // ID local da lista
  const navigate = useNavigate();
  const listId = parseInt(id);

  const [searchQuery, setSearchQuery] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const online = useStore((state) => state.online);
  const user = useStore((state) => state.user);

  // Consulta reativa da Lista local
  const listaObj = useLiveQuery(() => db.listas.get(listId), [listId]);

  // Consulta reativa dos itens que estão na lista
  const itensNaLista = useLiveQuery(
    () => db.itensLista.where('listaId').equals(listId).toArray(),
    [listId]
  );

  // Consulta reativa do catálogo de itens para adicionar
  const catalogoItens = useLiveQuery(() => db.itens.toArray());

  // Sincronizar dados da lista do servidor
  const loadListItems = async () => {
    if (!online || !listaObj?.idRemote) return;
    try {
      setLoading(true);
      setError('');

      // GET /ItemLista/GetLista?nIdLista={idRemote}
      const response = await api.get(`/ItemLista/GetLista?nIdLista=${listaObj.idRemote}`);

      await db.transaction('rw', db.itensLista, async () => {
        // Remove os itens sincronizados existentes locais dessa lista
        const localItems = await db.itensLista.where('listaId').equals(listId).toArray();
        const syncedIds = localItems.filter(i => i.syncStatus === 'synced').map(i => i.id);
        await db.itensLista.bulkDelete(syncedIds);

        // Insere os retornados da API
        for (const itemApi of response) {
          // Achar o correspondente do item local
          const localItem = await db.itens.where('idRemote').equals(itemApi.ItemId).first();
          
          await db.itensLista.add({
            idRemote: itemApi.Id,
            listaId: listId,
            itemId: localItem ? localItem.id : null,
            itemIdRemote: itemApi.ItemId,
            nomeItem: itemApi.Nome,
            unidadeMedida: itemApi.UnidadeMedida,
            quantidade: itemApi.Quantidade,
            preco: itemApi.Preco,
            statusItemListaId: itemApi.StatusItemListaId,
            syncStatus: 'synced'
          });
        }
      });
    } catch (err) {
      console.error("Erro ao sincronizar itens da lista do backend:", err);
      setError("Erro ao carregar dados em tempo real da API. Modo offline ativo.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (listaObj) {
      loadListItems();
    }
  }, [online, listaObj?.idRemote]);

  // Adicionar item do catálogo à lista de compras
  const handleAddItemToList = async (itemCatalogo) => {
    setError('');
    const itemExistente = itensNaLista?.find(i => i.itemId === itemCatalogo.id);

    if (itemExistente) {
      // Se já está na lista, aumenta a quantidade
      await handleUpdateQuantidade(itemExistente.id, itemExistente.quantidade + 1);
      return;
    }

    try {
      const newItemLista = {
        listaId: listId,
        itemId: itemCatalogo.id,
        itemIdRemote: itemCatalogo.idRemote,
        nomeItem: itemCatalogo.nome,
        unidadeMedida: itemCatalogo.unidadeMedida || 'UN',
        quantidade: 1,
        preco: null,
        statusItemListaId: 2, // Solicitado/Pendente
        syncStatus: 'created'
      };

      // 1. Salvar no Dexie
      const localId = await db.itensLista.add(newItemLista);

      // 2. Se online, envia para o servidor
      if (online && listaObj?.idRemote && itemCatalogo.idRemote) {
        const payload = {
          listaId: listaObj.idRemote,
          usuarioLogadoId: user.Id,
          itensLista: [
            {
              itemId: itemCatalogo.idRemote,
              quantidade: 1
            }
          ]
        };

        const result = await api.post('/ItemLista/Atualizar', payload);
        // Atualiza o ID remoto retornado
        if (result && result > 0) {
          await loadListItems();
        }
      }
    } catch (err) {
      console.error(err);
      setError('Erro ao adicionar item na API. Salvo localmente.');
    }
  };

  // Atualizar quantidade de um item na lista
  const handleUpdateQuantidade = async (idLocal, novaQuantidade) => {
    if (novaQuantidade <= 0) {
      await handleDeleteItem(idLocal);
      return;
    }

    setError('');
    try {
      const item = await db.itensLista.get(idLocal);
      const status = item.syncStatus === 'created' ? 'created' : 'updated';
      
      // 1. Salvar local
      await db.itensLista.update(idLocal, {
        quantidade: novaQuantidade,
        syncStatus: status
      });

      // 2. Se online, sincronizar com a API
      if (online && listaObj?.idRemote) {
        const payload = {
          listaId: listaObj.idRemote,
          usuarioLogadoId: user.Id,
          itensLista: [
            {
              id: item.idRemote || null,
              itemId: item.itemIdRemote || null,
              quantidade: novaQuantidade
            }
          ]
        };
        await api.post('/ItemLista/Atualizar', payload);
        if (item.syncStatus === 'synced') {
          await db.itensLista.update(idLocal, { syncStatus: 'synced' });
        }
      }
    } catch (err) {
      console.error(err);
      setError('Erro ao atualizar quantidade no servidor. Salvo localmente.');
    }
  };

  // Remover item da lista
  const handleDeleteItem = async (idLocal) => {
    setError('');
    try {
      const item = await db.itensLista.get(idLocal);

      if (online && listaObj?.idRemote && item.idRemote) {
        // Envia para a API com quantidade = null para excluir
        const payload = {
          listaId: listaObj.idRemote,
          usuarioLogadoId: user.Id,
          itensLista: [
            {
              id: item.idRemote,
              quantidade: null // Indica exclusão no backend
            }
          ]
        };
        await api.post('/ItemLista/Atualizar', payload);
        await db.itensLista.delete(idLocal);
      } else {
        if (item.syncStatus === 'created') {
          await db.itensLista.delete(idLocal);
        } else {
          // Marca como deletado localmente
          await db.itensLista.update(idLocal, { syncStatus: 'deleted', quantidade: null });
        }
      }
    } catch (err) {
      console.error(err);
      setError('Erro ao remover item do servidor. Salvo localmente.');
    }
  };

  // Filtrar itens do catálogo com base na busca do usuário
  const filteredCatalogo = catalogoItens ? catalogoItens.filter(item => {
    if (item.syncStatus === 'deleted') return false;
    
    // Não exibir itens do catálogo que pertencem a ambientes diferentes desta lista
    if (item.unidadeId !== listaObj?.unidadeId) return false;

    return item.nome.toLowerCase().includes(searchQuery.toLowerCase());
  }) : [];

  // Itens que estão de fato ativos na lista de compras (não marcados como deletados offline)
  const activeItensNaLista = itensNaLista ? itensNaLista.filter(i => i.syncStatus !== 'deleted' && i.quantidade !== null) : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
      
      {/* Botão Voltar & Header */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginBottom: '20px' }}>
        <button 
          onClick={() => navigate('/')} 
          className="btn-text" 
          style={{ padding: '8px', borderRadius: 'var(--radius-sm)', display: 'inline-flex', color: 'var(--text-heading)' }}
        >
          <ArrowLeft size={20} />
        </button>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '20px' }}>
            {listaObj ? listaObj.nome : 'Carregando lista...'}
          </h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>
            Gerencie as quantidades e itens da lista de compras
          </p>
        </div>
      </div>

      {error && (
        <div style={{
          padding: '12px 16px',
          borderRadius: 'var(--radius-sm)',
          backgroundColor: 'rgba(239, 68, 68, 0.1)',
          color: 'var(--error)',
          fontSize: '14px',
          marginBottom: '20px'
        }}>
          {error}
        </div>
      )}

      {/* Conteúdo Principal */}
      <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
        
        {/* Seção 1: Itens Atuais na Lista */}
        <div className="glass" style={{ padding: '16px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
          <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '14px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <ShoppingBag size={16} style={{ color: 'var(--primary)' }} /> Itens da Lista ({activeItensNaLista.length})
          </h3>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {activeItensNaLista.length === 0 ? (
              <div style={{ textAlign: 'center', padding: '30px 10px', color: 'var(--text-muted)', fontSize: '13px' }}>
                Nenhum item adicionado ainda. Pesquise e adicione itens no catálogo abaixo!
              </div>
            ) : (
              activeItensNaLista.map((item) => (
                <div key={item.id} style={{
                  display: 'flex',
                  alignItems: 'center',
                  justifyContent: 'space-between',
                  padding: '10px 12px',
                  borderRadius: 'var(--radius-sm)',
                  border: '1px solid var(--border-color)',
                  backgroundColor: 'rgba(255,255,255,0.01)'
                }}>
                  <div style={{ display: 'flex', flexDirection: 'column' }}>
                    <span style={{ fontWeight: 600, color: 'var(--text-heading)', fontSize: '14px' }}>
                      {item.nomeItem}
                    </span>
                    <span style={{ fontSize: '11px', color: 'var(--text-muted)' }}>
                      Medida: {item.unidadeMedida}
                    </span>
                    {item.syncStatus !== 'synced' && (
                      <span style={{ fontSize: '10px', color: 'var(--warning)', fontWeight: 500 }}>
                        Aguardando sincronização ({item.syncStatus})
                      </span>
                    )}
                  </div>

                  {/* Controles de Quantidade Premium */}
                  <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
                    <div style={{
                      display: 'flex',
                      alignItems: 'center',
                      backgroundColor: 'var(--bg-app)',
                      border: '1px solid var(--border-color)',
                      borderRadius: 'var(--radius-full)',
                      padding: '2px 4px'
                    }}>
                      <button 
                        onClick={() => handleUpdateQuantidade(item.id, item.quantidade - 1)}
                        style={{
                          width: '28px',
                          height: '28px',
                          borderRadius: 'var(--radius-full)',
                          border: 'none',
                          backgroundColor: 'transparent',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          cursor: 'pointer',
                          color: 'var(--text-muted)'
                        }}
                      >
                        <Minus size={14} />
                      </button>
                      <span style={{ minWidth: '32px', textAlign: 'center', fontWeight: 600, fontSize: '14px', color: 'var(--text-heading)' }}>
                        {item.quantidade}
                      </span>
                      <button 
                        onClick={() => handleUpdateQuantidade(item.id, item.quantidade + 1)}
                        style={{
                          width: '28px',
                          height: '28px',
                          borderRadius: 'var(--radius-full)',
                          border: 'none',
                          backgroundColor: 'transparent',
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          cursor: 'pointer',
                          color: 'var(--text-muted)'
                        }}
                      >
                        <Plus size={14} />
                      </button>
                    </div>

                    <button 
                      onClick={() => handleDeleteItem(item.id)}
                      className="btn-text"
                      style={{ padding: '6px', color: 'var(--error)', borderRadius: 'var(--radius-sm)' }}
                      title="Remover"
                    >
                      <Trash2 size={16} />
                    </button>
                  </div>
                </div>
              ))
            )}
          </div>
        </div>

        {/* Seção 2: Catálogo de Itens para Adicionar */}
        <div className="glass" style={{ padding: '16px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
          <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '12px' }}>
            Adicionar do Catálogo
          </h3>

          {/* Campo de Busca */}
          <div style={{ position: 'relative', marginBottom: '14px' }}>
            <input
              type="text"
              placeholder="Pesquisar itens cadastrados..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              className="form-control"
              style={{ height: '38px', paddingLeft: '38px', fontSize: '13px' }}
            />
            <Search size={16} style={{
              position: 'absolute',
              left: '12px',
              top: '50%',
              transform: 'translateY(-50%)',
              color: 'var(--text-light)'
            }} />
          </div>

          {/* Listagem do Catálogo Filtrada */}
          <div style={{ display: 'flex', flexDirection: 'column', gap: '8px', maxHeight: '200px', overflowY: 'auto', paddingRight: '4px' }}>
            {filteredCatalogo.length === 0 ? (
              <div style={{ textAlign: 'center', padding: '20px 0', color: 'var(--text-muted)', fontSize: '12px' }}>
                Nenhum item correspondente encontrado para este ambiente. Crie novos itens na aba "Itens".
              </div>
            ) : (
              filteredCatalogo.map((item) => {
                const jaAdicionado = activeItensNaLista.some(i => i.itemId === item.id);
                return (
                  <div 
                    key={item.id} 
                    onClick={() => handleAddItemToList(item)}
                    style={{
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'space-between',
                      padding: '8px 12px',
                      borderRadius: 'var(--radius-sm)',
                      border: jaAdicionado ? '1px solid var(--primary-glow)' : '1px solid var(--border-color)',
                      backgroundColor: jaAdicionado ? 'var(--primary-light)' : 'rgba(255, 255, 255, 0.01)',
                      cursor: 'pointer',
                      transition: 'all var(--transition-fast)'
                    }}
                  >
                    <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                      <span style={{ fontWeight: 500, fontSize: '13px', color: jaAdicionado ? 'var(--primary)' : 'var(--text-heading)' }}>
                        {item.nome}
                      </span>
                      <span style={{ fontSize: '9px', fontWeight: 600, color: 'var(--text-muted)', backgroundColor: 'var(--bg-app)', padding: '2px 4px', borderRadius: '4px' }}>
                        {item.unidadeMedida}
                      </span>
                    </div>
                    
                    <button 
                      className="btn-text" 
                      style={{ padding: '2px 8px', fontSize: '12px', fontWeight: 600, pointerEvents: 'none' }}
                    >
                      {jaAdicionado ? 'Adicionado (+1)' : '+ Adicionar'}
                    </button>
                  </div>
                );
              })
            )}
          </div>
        </div>

      </div>

    </div>
  );
}
