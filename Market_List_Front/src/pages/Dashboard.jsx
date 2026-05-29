import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { Plus, Trash2, ShoppingCart, ClipboardList, MapPin, Layers, Calendar, User } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function Dashboard() {
  const navigate = useNavigate();
  const [nomeLista, setNomeLista] = useState('');
  const [ambienteId, setAmbienteId] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  const online = useStore((state) => state.online);
  const user = useStore((state) => state.user);

  // Queries locais do Dexie
  const locaisListas = useLiveQuery(() => db.listas.toArray());
  const locaisAmbientes = useLiveQuery(() => db.ambientes.toArray());

  // Estado para os agrupamentos (listas mestras) do servidor
  const [agrupadores, setAgrupadores] = useState([]);

  // Carregar Unidades e Listas e Agrupadores
  const loadDashboardData = async () => {
    if (!online || !user?.Id) return;
    try {
      setLoading(true);
      setError('');

      // 1. Carregar unidades e listas
      const unidadesEListas = await api.get(`/Unidade/Listas/${user.Id}`);
      
      await db.transaction('rw', [db.ambientes, db.listas], async () => {
        // Limpar dados sincronizados antigos antes de reinserir
        const localAmbs = await db.ambientes.toArray();
        const localLists = await db.listas.toArray();

        // Deletamos apenas os já sincronizados para preservar os criados offline pendentes
        const syncedAmbsIds = localAmbs.filter(a => a.syncStatus === 'synced').map(a => a.id);
        const syncedListsIds = localLists.filter(l => l.syncStatus === 'synced').map(l => l.id);

        await db.ambientes.bulkDelete(syncedAmbsIds);
        await db.listas.bulkDelete(syncedListsIds);

        for (const uni of unidadesEListas) {
          // Verifica se já existe ambiente com esse idRemote
          let ambLocal = await db.ambientes.where('idRemote').equals(uni.Id).first();
          let ambLocalId;
          
          if (!ambLocal) {
            ambLocalId = await db.ambientes.add({
              idRemote: uni.Id,
              nome: uni.Nome,
              syncStatus: 'synced'
            });
          } else {
            ambLocalId = ambLocal.id;
          }

          if (uni.Listas && uni.Listas.length > 0) {
            const listRecords = uni.Listas.map(lista => ({
              idRemote: lista.Id,
              nome: lista.Nome,
              unidadeId: ambLocalId, // mapeia para chave primária local do ambiente
              syncStatus: 'synced'
            }));
            await db.listas.bulkAdd(listRecords);
          }
        }
      });

      // 2. Carregar agrupadores (Listas Mestras) do usuário
      const agrupados = await api.get(`/Lista/Agrupadas/UsuarioId/${user.Id}`);
      setAgrupadores(agrupados || []);
    } catch (err) {
      console.error("Erro ao carregar dados do Dashboard:", err);
      setError("Erro ao sincronizar com o servidor. Exibindo dados locais offline.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadDashboardData();
  }, [online, user?.Id]);

  const handleCreateLista = async (e) => {
    e.preventDefault();
    if (!nomeLista.trim() || !ambienteId) {
      setError('Informe o Nome da Lista e selecione um Ambiente.');
      return;
    }
    setLoading(true);
    setError('');

    const targetAmbienteId = parseInt(ambienteId);

    try {
      if (online) {
        // Encontra o ID remoto do ambiente
        const amb = await db.ambientes.get(targetAmbienteId);
        const remoteAmbId = amb?.idRemote || targetAmbienteId;

        // Envia para o backend
        await api.post('/Lista/SalvarLista', {
          nome: nomeLista.trim(),
          usuarioId: user.Id,
          unidadeId: remoteAmbId,
          itensLista: [] // inicializa vazia
        });

        await loadDashboardData();
      } else {
        // Salva localmente offline
        await db.listas.add({
          nome: nomeLista.trim(),
          unidadeId: targetAmbienteId,
          syncStatus: 'created'
        });
      }
      setNomeLista('');
      setAmbienteId('');
    } catch (err) {
      setError(err.message || 'Erro ao criar lista.');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteLista = async (id, idRemote) => {
    if (!window.confirm('Tem certeza que deseja excluir esta lista? Todos os itens dela serão perdidos.')) return;
    setLoading(true);
    setError('');

    try {
      if (online && idRemote) {
        await api.delete(`/Lista/Deletar/${idRemote}`);
        await db.listas.delete(id);
      } else {
        const list = await db.listas.get(id);
        if (list.syncStatus === 'created') {
          await db.listas.delete(id);
        } else {
          await db.listas.update(id, { syncStatus: 'deleted' });
        }
      }
      // Opcional: remover itensLista vinculados localmente
      await db.itensLista.where('listaId').equals(id).delete();
      await loadDashboardData();
    } catch (err) {
      setError(err.message || 'Erro ao excluir lista.');
    } finally {
      setLoading(false);
    }
  };

  // Filtragem dos ambientes ativos e listas
  const activeAmbientes = locaisAmbientes ? locaisAmbientes.filter(a => a.syncStatus !== 'deleted') : [];
  const activeListas = locaisListas ? locaisListas.filter(l => l.syncStatus !== 'deleted') : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
      
      {/* Bloco de Boas Vindas */}
      <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '24px' }}>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '22px' }}>Minhas Listas</h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>Gerencie suas listas de compras e agrupamentos</p>
        </div>
        <button 
          onClick={() => navigate('/merge')} 
          className="btn btn-secondary" 
          style={{ padding: '8px 14px', borderRadius: 'var(--radius-md)', fontSize: '13px', display: 'flex', alignItems: 'center', gap: '6px' }}
        >
          <Layers size={16} /> Unir Listas
        </button>
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

      {/* Form Criar Lista */}
      <div className="glass" style={{ padding: '20px', borderRadius: 'var(--radius-md)', marginBottom: '24px', boxShadow: 'var(--shadow-sm)' }}>
        <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '14px', display: 'flex', alignItems: 'center', gap: '6px' }}>
          <Plus size={18} style={{ color: 'var(--primary)' }} /> Criar Nova Lista
        </h3>
        <form onSubmit={handleCreateLista} style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
          <div style={{ display: 'flex', gap: '10px' }}>
            <input
              type="text"
              required
              placeholder="Ex: Compras da Semana, Lanche..."
              value={nomeLista}
              onChange={(e) => setNomeLista(e.target.value)}
              className="form-control"
              style={{ flex: 1, height: '40px', fontSize: '14px' }}
            />
            <select
              required
              value={ambienteId}
              onChange={(e) => setAmbienteId(e.target.value)}
              className="form-control"
              style={{ width: '150px', height: '40px', fontSize: '13px', padding: '0 8px' }}
            >
              <option value="">Ambiente...</option>
              {activeAmbientes.map(a => (
                <option key={a.id} value={a.id}>{a.nome}</option>
              ))}
            </select>
          </div>
          <button type="submit" disabled={loading} className="btn btn-primary btn-full" style={{ height: '40px' }}>
            <ClipboardList size={18} /> Criar Lista de Compras
          </button>
        </form>
      </div>

      {/* Listagem de Listas Mestras (Agrupadores) - Se houver e estiver online */}
      {agrupadores.length > 0 && (
        <div style={{ marginBottom: '24px' }}>
          <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '12px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <Layers size={18} style={{ color: 'var(--primary)' }} /> Listas Mestras (Unificadas)
          </h3>
          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {agrupadores.map((agrup) => (
              <div 
                key={agrup.Id} 
                className="glass" 
                style={{ 
                  padding: '16px', 
                  borderRadius: 'var(--radius-md)', 
                  border: '1px solid var(--primary-glow)',
                  boxShadow: 'var(--shadow-sm)',
                  display: 'flex', 
                  justifyContent: 'space-between', 
                  alignItems: 'center' 
                }}
              >
                <div>
                  <h4 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', margin: '0 0 4px 0' }}>
                    {agrup.Nome}
                  </h4>
                  <span style={{ fontSize: '12px', color: 'var(--text-muted)' }}>
                    {agrup.Itens?.length || 0} itens consolidados
                  </span>
                </div>
                <button
                  onClick={() => navigate(`/shopping/master-${agrup.Id}`, { state: { agrupador: agrup } })}
                  className="btn btn-primary"
                  style={{ padding: '8px 14px', borderRadius: 'var(--radius-sm)', fontSize: '13px', display: 'flex', alignItems: 'center', gap: '6px' }}
                >
                  <ShoppingCart size={15} /> Comprar
                </button>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* Listagem por Ambiente */}
      <div>
        <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '12px', display: 'flex', alignItems: 'center', gap: '8px' }}>
          <MapPin size={18} style={{ color: 'var(--primary)' }} /> Listas por Ambiente
        </h3>

        {activeAmbientes.length === 0 ? (
          <div style={{ textAlign: 'center', padding: '30px 20px', color: 'var(--text-muted)', fontSize: '14px', border: '2px dashed var(--border-color)', borderRadius: 'var(--radius-md)' }}>
            Nenhum ambiente cadastrado. Vá até a aba "Ambientes" para criar um local.
          </div>
        ) : (
          <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
            {activeAmbientes.map((amb) => {
              const listasDoAmbiente = activeListas.filter(l => l.unidadeId === amb.id);

              return (
                <div key={amb.id} className="glass" style={{ padding: '16px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
                  <h4 style={{ fontSize: '14px', fontWeight: 700, color: 'var(--primary)', marginBottom: '12px', display: 'flex', alignItems: 'center', gap: '6px', borderBottom: '1px solid var(--border-color)', paddingBottom: '6px' }}>
                    <MapPin size={14} /> {amb.nome}
                  </h4>

                  {listasDoAmbiente.length === 0 ? (
                    <div style={{ color: 'var(--text-light)', fontSize: '12px', textAlign: 'center', padding: '12px 0' }}>
                      Nenhuma lista de compras neste ambiente.
                    </div>
                  ) : (
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
                      {listasDoAmbiente.map((lista) => (
                        <div key={lista.id} style={{
                          display: 'flex',
                          alignItems: 'center',
                          justifyContent: 'space-between',
                          padding: '10px 12px',
                          borderRadius: 'var(--radius-sm)',
                          border: '1px solid var(--border-color)',
                          backgroundColor: 'rgba(255, 255, 255, 0.01)'
                        }}>
                          <div 
                            style={{ display: 'flex', flexDirection: 'column', cursor: 'pointer', flex: 1 }}
                            onClick={() => navigate(`/list/${lista.id}`)}
                          >
                            <span style={{ fontWeight: 600, color: 'var(--text-heading)', fontSize: '14px' }}>
                              {lista.nome}
                            </span>
                            {lista.syncStatus !== 'synced' && (
                              <span style={{ fontSize: '10px', color: 'var(--warning)', fontWeight: 500 }}>
                                Aguardando sincronização
                              </span>
                            )}
                          </div>
                          
                          <div style={{ display: 'flex', alignItems: 'center', gap: '4px' }}>
                            <button
                              onClick={() => navigate(`/shopping/${lista.id}`)}
                              className="btn btn-primary"
                              style={{ padding: '6px 10px', borderRadius: 'var(--radius-sm)', fontSize: '12px', display: 'flex', alignItems: 'center', gap: '4px', height: '32px' }}
                              title="Iniciar compras"
                            >
                              <ShoppingCart size={14} /> Comprar
                            </button>
                            <button
                              onClick={() => handleDeleteLista(lista.id, lista.idRemote)}
                              className="btn-text"
                              style={{ padding: '6px', color: 'var(--error)', borderRadius: 'var(--radius-sm)', display: 'inline-flex', alignItems: 'center', justifyContent: 'center' }}
                              title="Excluir"
                            >
                              <Trash2 size={16} />
                            </button>
                          </div>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        )}
      </div>

    </div>
  );
}
