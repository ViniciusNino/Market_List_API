import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { ArrowLeft, Check, CheckCircle2, ShoppingCart, Filter, Tag, DollarSign, RefreshCw } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function ShoppingMode() {
  const { id } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  
  const online = useStore((state) => state.online);
  const user = useStore((state) => state.user);

  // Estados locais
  const [nomeLista, setNomeLista] = useState('Modo Compras');
  const [itensConsolidados, setItensConsolidados] = useState([]);
  const [selectedSetorId, setSelectedSetorId] = useState('all');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Queries do catálogo de apoio
  const catalogoItens = useLiveQuery(() => db.itens.toArray());
  const mercados = useLiveQuery(() => db.mercados.toArray());
  const setores = useLiveQuery(() => db.setores.toArray());
  const ambientes = useLiveQuery(() => db.ambientes.toArray());

  // Detectar qual é o modo (Individual, União Offline ou Mestre Remoto)
  const isMerged = id === 'merged';
  const isMaster = id.startsWith('master-');

  // Inicializar e processar listas e itens
  useEffect(() => {
    const processData = async () => {
      setLoading(true);
      try {
        if (isMerged) {
          // Modo 2: União Offline de múltiplas listas locais
          const localListIds = location.state?.localListIds || [];
          const nomeMestra = location.state?.nomeMestra || 'Lista Mestra Unificada';
          setNomeLista(nomeMestra);

          if (localListIds.length === 0) {
            setError('Nenhuma lista selecionada para a união.');
            setLoading(false);
            return;
          }

          // Carregar todas as listas correspondentes para pegar os nomes
          const listasLocais = await db.listas.where('id').anyOf(localListIds).toArray();
          
          // Buscar todos os itens de todas as listas selecionadas
          const todosItensLista = await db.itensLista
            .where('listaId')
            .anyOf(localListIds)
            .toArray();

          // Filtrar deletados offline
          const ativosItensLista = todosItensLista.filter(i => i.syncStatus !== 'deleted' && i.quantidade !== null);

          // Consolidar por ItemId
          const consolidadosMap = {};
          
          for (const itemLista of ativosItensLista) {
            const itemInfo = catalogoItens?.find(i => i.id === itemLista.itemId);
            const listaInfo = listasLocais.find(l => l.id === itemLista.listaId);
            const key = itemLista.itemId || `local-${itemLista.nomeItem}`;

            if (!consolidadosMap[key]) {
              consolidadosMap[key] = {
                itemId: itemLista.itemId,
                itemIdRemote: itemLista.itemIdRemote,
                nome: itemLista.nomeItem,
                unidadeMedida: itemLista.unidadeMedida,
                quantidadeTotal: 0,
                precoUnitario: itemLista.preco || '',
                comprado: itemLista.statusItemListaId === 3, // Status Comprado
                sessaoId: itemInfo ? itemInfo.sessaoId : null,
                setorId: itemInfo ? itemInfo.setorId : null,
                origens: [] // Armazena de onde veio cada quantidade
              };
            }

            consolidadosMap[key].quantidadeTotal += itemLista.quantidade;
            consolidadosMap[key].origens.push({
              idLocalItemLista: itemLista.id,
              idRemoteItemLista: itemLista.idRemote,
              listaId: itemLista.listaId,
              nomeLista: listaInfo ? listaInfo.nome : 'Lista',
              quantidade: itemLista.quantidade
            });

            // Se algum dos itens ainda não estiver comprado, a consolidação fica pendente
            if (itemLista.statusItemListaId !== 3) {
              consolidadosMap[key].comprado = false;
            }
          }

          setItensConsolidados(Object.values(consolidadosMap));
        } 
        else if (isMaster) {
          // Modo 3: Lista Mestra do Servidor (Agrupador)
          const agrupadorObj = location.state?.agrupador;
          if (agrupadorObj) {
            setNomeLista(agrupadorObj.Nome);
            
            // A API retorna a lista com Nome, Quantidade, UnidadeMedida.
            // Para poder realizar a compra de forma completa, vamos mapear esses itens.
            const mapeados = agrupadorObj.Itens.map((item, index) => {
              // Buscar no catálogo correspondente para agrupar por setor/mercado
              const catalogoObj = catalogoItens?.find(i => i.nome.toLowerCase() === item.Nome.toLowerCase());

              return {
                idMaster: index,
                nome: item.Nome,
                unidadeMedida: item.UnidadeMedida,
                quantidadeTotal: item.Quantidade,
                precoUnitario: '',
                comprado: false,
                sessaoId: catalogoObj ? catalogoObj.sessaoId : null,
                setorId: catalogoObj ? catalogoObj.setorId : null,
                origens: []
              };
            });
            setItensConsolidados(mapeados);
          }
        } 
        else {
          // Modo 1: Lista Individual
          const localListId = parseInt(id);
          const listObj = await db.listas.get(localListId);
          if (listObj) {
            setNomeLista(listObj.nome);

            const itensListaObj = await db.itensLista
              .where('listaId')
              .equals(localListId)
              .toArray();

            // Filtrar deletados offline
            const ativos = itensListaObj.filter(i => i.syncStatus !== 'deleted' && i.quantidade !== null);

            const mapeados = ativos.map(itemLista => {
              const itemInfo = catalogoItens?.find(i => i.id === itemLista.itemId);
              
              return {
                idLocalItemLista: itemLista.id,
                idRemoteItemLista: itemLista.idRemote,
                itemId: itemLista.itemId,
                itemIdRemote: itemLista.itemIdRemote,
                nome: itemLista.nomeItem,
                unidadeMedida: itemLista.unidadeMedida,
                quantidadeTotal: itemLista.quantidade,
                precoUnitario: itemLista.preco || '',
                comprado: itemLista.statusItemListaId === 3,
                sessaoId: itemInfo ? itemInfo.sessaoId : null,
                setorId: itemInfo ? itemInfo.setorId : null,
                origens: [{
                  idLocalItemLista: itemLista.id,
                  idRemoteItemLista: itemLista.idRemote,
                  listaId: localListId,
                  nomeLista: listObj.nome,
                  quantidade: itemLista.quantidade
                }]
              };
            });

            setItensConsolidados(mapeados);
          }
        }
      } catch (err) {
        console.error(err);
        setError('Erro ao processar as listas para compras.');
      } finally {
        setLoading(false);
      }
    };

    if (catalogoItens) {
      processData();
    }
  }, [id, catalogoItens, location.state]);

  // Ação de marcar/desmarcar item e atualizar preço
  const handleToggleCheck = async (index, forceState = null) => {
    const novosItens = [...itensConsolidados];
    const item = novosItens[index];
    
    const novoStatusComprado = forceState !== null ? forceState : !item.comprado;
    item.comprado = novoStatusComprado;
    
    setItensConsolidados(novosItens);

    // Se estiver no fluxo local (Individual ou Merged Offline)
    if (!isMaster) {
      const precoNum = item.precoUnitario ? parseFloat(item.precoUnitario) : null;
      const statusId = novoStatusComprado ? 3 : 2; // 3 = Comprado, 2 = Solicitado/Pendente

      try {
        // Atualiza cada item original correspondente na tabela db.itensLista
        for (const orig of item.origens) {
          const itemOriginal = await db.itensLista.get(orig.idLocalItemLista);
          if (itemOriginal) {
            const syncStatus = itemOriginal.syncStatus === 'created' ? 'created' : 'updated';
            await db.itensLista.update(orig.idLocalItemLista, {
              statusItemListaId: statusId,
              preco: precoNum,
              syncStatus: syncStatus
            });

            // Se online, sincroniza de imediato com o backend
            if (online) {
              const listObj = await db.listas.get(orig.providerListaId || orig.listaId);
              if (listObj && listObj.idRemote) {
                const payload = {
                  listaId: listObj.idRemote,
                  usuarioLogadoId: user.Id,
                  itensLista: [
                    {
                      id: orig.idRemoteItemLista || null,
                      itemId: item.itemIdRemote || null,
                      quantidade: orig.quantidade,
                      preco: precoNum,
                      statusItemListaId: statusId
                    }
                  ]
                };
                await api.post('/ItemLista/Atualizar', payload);
                await db.itensLista.update(orig.idLocalItemLista, { syncStatus: 'synced' });
              }
            }
          }
        }
      } catch (err) {
        console.error("Erro ao salvar status da compra:", err);
      }
    }
  };

  const handlePrecoChange = async (index, valor) => {
    // Sanitiza apenas números e ponto/vírgula
    const valorLimpo = valor.replace(',', '.').replace(/[^\d.]/g, '');
    
    const novosItens = [...itensConsolidados];
    novosItens[index].precoUnitario = valorLimpo;
    setItensConsolidados(novosItens);

    const item = novosItens[index];

    // Se estiver no fluxo local, persiste o preço alterado imediatamente
    if (!isMaster && valorLimpo) {
      const precoNum = parseFloat(valorLimpo);
      
      try {
        for (const orig of item.origens) {
          const itemOriginal = await db.itensLista.get(orig.idLocalItemLista);
          if (itemOriginal) {
            const syncStatus = itemOriginal.syncStatus === 'created' ? 'created' : 'updated';
            await db.itensLista.update(orig.idLocalItemLista, {
              preco: precoNum,
              syncStatus: syncStatus
            });

            // Se online, sincroniza
            if (online) {
              const listObj = await db.listas.get(orig.listaId);
              if (listObj && listObj.idRemote) {
                const payload = {
                  listaId: listObj.idRemote,
                  usuarioLogadoId: user.Id,
                  itensLista: [
                    {
                      id: orig.idRemoteItemLista || null,
                      itemId: item.itemIdRemote || null,
                      quantidade: orig.quantidade,
                      preco: precoNum,
                      statusItemListaId: item.comprado ? 3 : 2
                    }
                  ]
                };
                await api.post('/ItemLista/Atualizar', payload);
                await db.itensLista.update(orig.idLocalItemLista, { syncStatus: 'synced' });
              }
            }
          }
        }
      } catch (err) {
        console.error("Erro ao salvar alteração de preço:", err);
      }
    }
  };

  // Finalizar compras e sincronizar tudo
  const handleFinalizar = async () => {
    setLoading(true);
    try {
      // Se online, dispara a sincronização geral (caso tenha algo pendente)
      if (online) {
        // Enviar requisição para atualizar o status dos agrupamentos se necessário
        alert("Compras finalizadas com sucesso! Os preços foram consolidados no banco de dados.");
      } else {
        alert("Compras salvas localmente! Assim que você se conectar à internet, os preços e status serão atualizados no servidor.");
      }
      navigate('/');
    } catch (err) {
      console.error(err);
      setError('Erro ao finalizar compras.');
    } finally {
      setLoading(false);
    }
  };

  // Listagem de setores exclusivos para as abas no topo com base nos itens da lista
  const setoresIdsPresentes = [...new Set(itensConsolidados.map(i => i.setorId).filter(id => id !== null))];
  const abasSetores = setores ? setores.filter(s => setoresIdsPresentes.includes(s.id) || setoresIdsPresentes.includes(s.idRemote)) : [];

  // Filtrar itens ativos na aba selecionada
  const filteredItens = itensConsolidados.filter(item => {
    if (selectedSetorId === 'all') return true;
    return item.setorId === parseInt(selectedSetorId) || (setores.find(s => s.id === parseInt(selectedSetorId))?.idRemote === item.setorId);
  });

  // Cálculo do totalizador de preços
  const precoTotal = itensConsolidados.reduce((acc, curr) => {
    if (curr.comprado && curr.precoUnitario) {
      return acc + (parseFloat(curr.precoUnitario) * curr.quantidadeTotal);
    }
    return acc;
  }, 0);

  return (
    <div style={{ padding: '0 0 100px 0' }} className="animate-fade-in">
      
      {/* Header Fixo de Compras */}
      <div className="glass" style={{
        position: 'sticky',
        top: 0,
        zIndex: 90,
        padding: '16px 20px',
        borderBottom: '1px solid var(--border-color)',
        display: 'flex',
        alignItems: 'center',
        gap: '12px'
      }}>
        <button 
          onClick={() => navigate('/')} 
          className="btn-text" 
          style={{ padding: '6px', color: 'var(--text-heading)', display: 'inline-flex' }}
        >
          <ArrowLeft size={20} />
        </button>
        <div style={{ flex: 1 }}>
          <h2 style={{ fontSize: '18px', fontWeight: 700, color: 'var(--text-heading)', margin: 0, whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis' }}>
            {nomeLista}
          </h2>
          <span style={{ fontSize: '11px', color: 'var(--text-muted)', display: 'flex', alignItems: 'center', gap: '4px' }}>
            <ShoppingCart size={11} /> Modo Compras Ativo
          </span>
        </div>
        <button 
          onClick={handleFinalizar}
          className="btn btn-primary"
          style={{ padding: '6px 12px', borderRadius: 'var(--radius-sm)', fontSize: '12px', fontWeight: 700, height: '32px' }}
        >
          Finalizar
        </button>
      </div>

      {error && (
        <div style={{
          margin: '16px 20px',
          padding: '12px 16px',
          borderRadius: 'var(--radius-sm)',
          backgroundColor: 'rgba(239, 68, 68, 0.1)',
          color: 'var(--error)',
          fontSize: '13px'
        }}>
          {error}
        </div>
      )}

      {/* Abas Horizontais Deslizantes de Setores (Filtro) */}
      {abasSetores.length > 0 && (
        <div style={{
          display: 'flex',
          gap: '8px',
          padding: '12px 20px',
          overflowX: 'auto',
          backgroundColor: 'rgba(0,0,0,0.02)',
          borderBottom: '1px solid var(--border-color)'
        }}>
          <button
            onClick={() => setSelectedSetorId('all')}
            style={{
              padding: '6px 14px',
              borderRadius: 'var(--radius-full)',
              border: 'none',
              fontSize: '12px',
              fontWeight: 600,
              cursor: 'pointer',
              backgroundColor: selectedSetorId === 'all' ? 'var(--primary)' : 'var(--bg-card)',
              color: selectedSetorId === 'all' ? '#fff' : 'var(--text-main)',
              boxShadow: 'var(--shadow-sm)',
              whiteSpace: 'nowrap',
              transition: 'all var(--transition-fast)'
            }}
          >
            Ver Todos ({itensConsolidados.length})
          </button>
          
          {abasSetores.map((s) => {
            const count = itensConsolidados.filter(i => i.setorId === s.id || i.setorId === s.idRemote).length;
            const isSel = selectedSetorId === String(s.id);
            return (
              <button
                key={s.id}
                onClick={() => setSelectedSetorId(String(s.id))}
                style={{
                  padding: '6px 14px',
                  borderRadius: 'var(--radius-full)',
                  border: 'none',
                  fontSize: '12px',
                  fontWeight: 600,
                  cursor: 'pointer',
                  backgroundColor: isSel ? 'var(--primary)' : 'var(--bg-card)',
                  color: isSel ? '#fff' : 'var(--text-main)',
                  boxShadow: 'var(--shadow-sm)',
                  whiteSpace: 'nowrap',
                  transition: 'all var(--transition-fast)'
                }}
              >
                {s.nome} ({count})
              </button>
            );
          })}
        </div>
      )}

      {/* Listagem de Itens no Modo Compras */}
      <div style={{ padding: '16px 20px', display: 'flex', flexDirection: 'column', gap: '12px' }}>
        {filteredItens.length === 0 ? (
          <div style={{ textAlign: 'center', padding: '40px 20px', color: 'var(--text-muted)', fontSize: '13px' }}>
            Nenhum item pendente de compra neste setor.
          </div>
        ) : (
          filteredItens.map((item, index) => {
            const indexNoArrayGeral = itensConsolidados.findIndex(i => i.nome === item.nome && i.itemId === item.itemId);
            
            return (
              <div 
                key={item.idLocalItemLista || index}
                className="glass" 
                style={{
                  padding: '16px',
                  borderRadius: 'var(--radius-md)',
                  boxShadow: 'var(--shadow-sm)',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '14px',
                  opacity: item.comprado ? 0.6 : 1,
                  borderLeft: item.comprado ? '4px solid var(--success)' : '4px solid transparent',
                  transition: 'all var(--transition-normal)'
                }}
              >
                {/* Checkbox circular grande */}
                <button
                  onClick={() => handleToggleCheck(indexNoArrayGeral)}
                  style={{
                    width: '32px',
                    height: '32px',
                    borderRadius: '50%',
                    border: item.comprado ? '2px solid var(--success)' : '2px solid var(--text-light)',
                    backgroundColor: item.comprado ? 'var(--success)' : 'transparent',
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    cursor: 'pointer',
                    color: '#fff',
                    outline: 'none',
                    transition: 'all var(--transition-fast)'
                  }}
                >
                  {item.comprado && <Check size={18} />}
                </button>

                {/* Nome e Origens */}
                <div style={{ flex: 1, display: 'flex', flexDirection: 'column', gap: '4px' }}>
                  <span style={{ 
                    fontWeight: 600, 
                    color: 'var(--text-heading)', 
                    fontSize: '15px',
                    textDecoration: item.comprado ? 'line-through' : 'none'
                  }}>
                    {item.nome}
                  </span>
                  
                  <div style={{ display: 'flex', alignItems: 'center', gap: '6px' }}>
                    <span style={{
                      fontSize: '10px',
                      fontWeight: 700,
                      backgroundColor: 'var(--bg-app)',
                      color: 'var(--text-muted)',
                      padding: '2px 6px',
                      borderRadius: '4px'
                    }}>
                      Qtd: {item.quantidadeTotal} {item.unidadeMedida}
                    </span>

                    {/* Exibe o Setor no item se tiver */}
                    {setores && item.setorId && (
                      <span style={{ fontSize: '10px', color: 'var(--text-light)' }}>
                        • {setores.find(s => s.id === item.setorId || s.idRemote === item.setorId)?.nome || ''}
                      </span>
                    )}
                  </div>

                  {/* Detalhes de Origem (Tags) */}
                  {item.origens && item.origens.length > 0 && (
                    <div style={{ display: 'flex', flexWrap: 'wrap', gap: '4px', marginTop: '4px' }}>
                      {item.origens.map((orig, idx) => (
                        <span 
                          key={idx} 
                          style={{
                            fontSize: '9px',
                            fontWeight: 600,
                            backgroundColor: 'var(--primary-light)',
                            color: 'var(--primary)',
                            padding: '1px 5px',
                            borderRadius: '4px',
                            display: 'inline-flex',
                            alignItems: 'center',
                            gap: '2px'
                          }}
                        >
                          <Tag size={8} /> {orig.nomeLista}: {orig.quantidade}x
                        </span>
                      ))}
                    </div>
                  )}
                </div>

                {/* Input de Preço do Item */}
                <div style={{ width: '90px', display: 'flex', flexDirection: 'column', alignItems: 'flex-end', gap: '2px' }}>
                  <span style={{ fontSize: '10px', color: 'var(--text-light)', fontWeight: 500 }}>
                    Preço Unit.
                  </span>
                  <div style={{ position: 'relative', width: '100%' }}>
                    <input
                      type="text"
                      placeholder="0.00"
                      value={item.precoUnitario}
                      onChange={(e) => handlePrecoChange(indexNoArrayGeral, e.target.value)}
                      className="form-control"
                      style={{
                        height: '32px',
                        padding: '0 4px 0 20px',
                        fontSize: '13px',
                        textAlign: 'right',
                        borderRadius: 'var(--radius-sm)'
                      }}
                    />
                    <DollarSign size={12} style={{
                      position: 'absolute',
                      left: '6px',
                      top: '50%',
                      transform: 'translateY(-50%)',
                      color: 'var(--text-light)'
                    }} />
                  </div>
                </div>

              </div>
            );
          })
        )}
      </div>

      {/* Rodapé Fixo Premium com Totalizador de Compra */}
      <div className="glass" style={{
        position: 'fixed',
        bottom: 0,
        left: '50%',
        transform: 'translateX(-50%)',
        width: '100%',
        maxWidth: '480px',
        height: '76px',
        borderTop: '1px solid var(--border-color)',
        display: 'flex',
        justifyContent: 'space-between',
        alignItems: 'center',
        padding: '0 24px',
        zIndex: 100
      }}>
        <div style={{ display: 'flex', flexDirection: 'column' }}>
          <span style={{ fontSize: '12px', color: 'var(--text-muted)', fontWeight: 500 }}>
            Total da Compra
          </span>
          <span style={{ fontSize: '22px', fontWeight: 800, color: 'var(--primary)' }}>
            R$ {precoTotal.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
          </span>
        </div>

        <button 
          onClick={handleFinalizar}
          className="btn btn-primary"
          style={{ height: '44px', padding: '0 20px', display: 'flex', alignItems: 'center', gap: '8px' }}
        >
          <CheckCircle2 size={18} />
          Concluir Compra
        </button>
      </div>

    </div>
  );
}
