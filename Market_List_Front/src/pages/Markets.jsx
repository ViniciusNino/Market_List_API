import React, { useState, useEffect } from 'react';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { Plus, Trash2, Edit2, Check, X, Store, ArrowRight, Layers, Tag } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function Markets() {
  const [mercadoNome, setMercadoNome] = useState('');
  const [editingMercadoId, setEditingMercadoId] = useState(null);
  const [editingMercadoNome, setEditingMercadoNome] = useState('');
  
  const [selectedMercado, setSelectedMercado] = useState(null); // Mercado selecionado para ver setores
  const [setorNome, setSetorNome] = useState('');
  const [editingSetorId, setEditingSetorId] = useState(null);
  const [editingSetorNome, setEditingSetorNome] = useState('');

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const online = useStore((state) => state.online);

  // Consulta reativa do Dexie local para Mercados e Setores
  const locaisMercados = useLiveQuery(() => db.mercados.toArray());
  const locaisSetores = useLiveQuery(() => db.setores.toArray());

  // Carregar dados da API
  const loadMarketsAndSectors = async () => {
    if (!online) return;
    try {
      setLoading(true);
      
      // Carregar Sessões (Mercados)
      const sessoes = await api.get('/Sessao');
      // Carregar Setores
      const setores = await api.get('/Setor');

      // Salva no Dexie
      await db.transaction('rw', [db.mercados, db.setores], async () => {
        await db.mercados.clear();
        await db.setores.clear();

        const mercadoRecords = sessoes.map(item => ({
          idRemote: item.Id,
          nome: item.SNome,
          syncStatus: 'synced'
        }));
        await db.mercados.bulkAdd(mercadoRecords);

        const setorRecords = setores.map(item => ({
          idRemote: item.Id,
          nome: item.SNome,
          sessaoId: item.NIdSessao,
          syncStatus: 'synced'
        }));
        await db.setores.bulkAdd(setorRecords);
      });
    } catch (err) {
      console.error("Erro ao sincronizar mercados e setores do backend:", err);
      setError('Erro ao carregar dados do servidor. Exibindo dados locais offline.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadMarketsAndSectors();
  }, [online]);

  // CRUD Mercados
  const handleCreateMercado = async (e) => {
    e.preventDefault();
    if (!mercadoNome.trim()) return;
    setLoading(true);
    setError('');

    try {
      if (online) {
        // Envia para o backend (Sessao)
        await api.post('/Sessao', {
          sNome: mercadoNome.trim()
        });
        await loadMarketsAndSectors();
      } else {
        // Salva offline no Dexie
        await db.mercados.add({
          nome: mercadoNome.trim(),
          syncStatus: 'created'
        });
      }
      setMercadoNome('');
    } catch (err) {
      setError(err.message || 'Erro ao criar mercado.');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateMercado = async (id, idRemote) => {
    if (!editingMercadoNome.trim()) return;
    setLoading(true);
    setError('');

    try {
      if (online && idRemote) {
        await api.put(`/Sessao/${idRemote}`, {
          id: idRemote,
          sNome: editingMercadoNome.trim()
        });
        await db.mercados.update(id, { nome: editingMercadoNome.trim(), syncStatus: 'synced' });
      } else {
        const item = await db.mercados.get(id);
        const status = item.syncStatus === 'created' ? 'created' : 'updated';
        await db.mercados.update(id, { nome: editingMercadoNome.trim(), syncStatus: status });
      }
      setEditingMercadoId(null);
    } catch (err) {
      setError(err.message || 'Erro ao editar mercado.');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteMercado = async (id, idRemote) => {
    if (!window.confirm('Deseja realmente remover este mercado? Todos os setores vinculados a ele serão excluídos.')) return;
    setLoading(true);
    setError('');

    try {
      if (online && idRemote) {
        await api.delete(`/Sessao/${idRemote}`);
        await db.mercados.delete(id);
        
        // Deletar também setores locais vinculados
        const setoresToDelete = await db.setores.where('sessaoId').equals(idRemote).toArray();
        for (const s of setoresToDelete) {
          await db.setores.delete(s.id);
        }
      } else {
        const item = await db.mercados.get(id);
        if (item.syncStatus === 'created') {
          await db.mercados.delete(id);
          // Remove setores criados localmente correspondentes
          const setoresToDelete = await db.setores.where('sessaoId').equals(id).toArray(); // offline id
          for (const s of setoresToDelete) {
            await db.setores.delete(s.id);
          }
        } else {
          await db.mercados.update(id, { syncStatus: 'deleted' });
          // Marca setores vinculados como deletados
          const targetId = idRemote || id;
          const setoresToMark = await db.setores.where('sessaoId').equals(targetId).toArray();
          for (const s of setoresToMark) {
            await db.setores.update(s.id, { syncStatus: 'deleted' });
          }
        }
      }

      if (selectedMercado && (selectedMercado.id === id || selectedMercado.idRemote === idRemote)) {
        setSelectedMercado(null);
      }
    } catch (err) {
      setError(err.message || 'Erro ao deletar mercado.');
    } finally {
      setLoading(false);
    }
  };

  // CRUD Setores
  const handleCreateSetor = async (e) => {
    e.preventDefault();
    if (!setorNome.trim() || !selectedMercado) return;
    setLoading(true);
    setError('');

    // Determina o ID do mercado a ser associado (remoto se sincronizado, local senão)
    const targetSessaoId = selectedMercado.idRemote || selectedMercado.id;

    try {
      if (online && selectedMercado.idRemote) {
        // Envia para o backend (Setor)
        await api.post('/Setor', {
          sNome: setorNome.trim(),
          nIdSessao: selectedMercado.idRemote
        });
        await loadMarketsAndSectors();
      } else {
        // Salva offline no Dexie
        await db.setores.add({
          nome: setorNome.trim(),
          sessaoId: targetSessaoId,
          syncStatus: 'created'
        });
      }
      setSetorNome('');
    } catch (err) {
      setError(err.message || 'Erro ao criar setor.');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateSetor = async (id, idRemote) => {
    if (!editingSetorNome.trim() || !selectedMercado) return;
    setLoading(true);
    setError('');

    const targetSessaoId = selectedMercado.idRemote || selectedMercado.id;

    try {
      if (online && idRemote && selectedMercado.idRemote) {
        await api.put(`/Setor/${idRemote}`, {
          id: idRemote,
          sNome: editingSetorNome.trim(),
          nIdSessao: selectedMercado.idRemote
        });
        await db.setores.update(id, { nome: editingSetorNome.trim(), syncStatus: 'synced' });
      } else {
        const item = await db.setores.get(id);
        const status = item.syncStatus === 'created' ? 'created' : 'updated';
        await db.setores.update(id, { nome: editingSetorNome.trim(), syncStatus: status });
      }
      setEditingSetorId(null);
    } catch (err) {
      setError(err.message || 'Erro ao editar setor.');
    } finally {
      setLoading(false);
    }
  };

  const handleDeleteSetor = async (id, idRemote) => {
    if (!window.confirm('Deseja realmente remover este setor?')) return;
    setLoading(true);
    setError('');

    try {
      if (online && idRemote) {
        await api.delete(`/Setor/${idRemote}`);
        await db.setores.delete(id);
      } else {
        const item = await db.setores.get(id);
        if (item.syncStatus === 'created') {
          await db.setores.delete(id);
        } else {
          await db.setores.update(id, { syncStatus: 'deleted' });
        }
      }
    } catch (err) {
      setError(err.message || 'Erro ao deletar setor.');
    } finally {
      setLoading(false);
    }
  };

  // Filtrar dados ativos
  const listMercados = locaisMercados ? locaisMercados.filter(m => m.syncStatus !== 'deleted') : [];
  
  // Filtrar setores do mercado selecionado
  const activeMercadoId = selectedMercado ? (selectedMercado.idRemote || selectedMercado.id) : null;
  const listSetores = (locaisSetores && activeMercadoId) 
    ? locaisSetores.filter(s => s.sessaoId === activeMercadoId && s.syncStatus !== 'deleted') 
    : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
      {/* Header da Tela */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginBottom: '24px' }}>
        <div style={{
          width: '40px',
          height: '40px',
          borderRadius: 'var(--radius-sm)',
          backgroundColor: 'var(--primary-light)',
          color: 'var(--primary)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center'
        }}>
          <Store size={22} />
        </div>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '22px' }}>
            Mercados & Setores
          </h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>
            Gerencie locais de compra e suas seções/corredores
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

      {/* Grid Principal Layout */}
      <div style={{ display: 'flex', flexDirection: 'column', gap: '24px' }}>
        
        {/* Bloco de Mercados */}
        <div className="glass" style={{ padding: '20px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
          <h3 style={{ fontSize: '16px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '16px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <Store size={18} style={{ color: 'var(--primary)' }} /> Mercados
          </h3>

          {/* Form Mercados */}
          <form onSubmit={handleCreateMercado} style={{ display: 'flex', gap: '8px', marginBottom: '16px' }}>
            <input
              type="text"
              required
              placeholder="Ex: Guanabara, Ceasa, Supermercado X..."
              value={mercadoNome}
              onChange={(e) => setMercadoNome(e.target.value)}
              className="form-control"
              style={{ flex: 1, height: '40px', padding: '0 12px', fontSize: '14px' }}
            />
            <button type="submit" disabled={loading} className="btn btn-primary" style={{ height: '40px', padding: '0 14px', borderRadius: 'var(--radius-md)' }}>
              <Plus size={18} />
              Adicionar
            </button>
          </form>

          {/* Listagem de Mercados */}
          <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
            {listMercados.length === 0 ? (
              <div style={{ textAlign: 'center', padding: '20px 10px', color: 'var(--text-muted)', fontSize: '13px' }}>
                Nenhum mercado cadastrado.
              </div>
            ) : (
              listMercados.map((m) => {
                const isSelected = selectedMercado && (selectedMercado.id === m.id);
                return (
                  <div key={m.id} style={{
                    display: 'flex',
                    flexDirection: 'column',
                    padding: '12px',
                    borderRadius: 'var(--radius-sm)',
                    border: isSelected ? '1px solid var(--primary)' : '1px solid var(--border-color)',
                    backgroundColor: isSelected ? 'var(--primary-light)' : 'rgba(255,255,255,0.02)',
                    transition: 'all var(--transition-fast)'
                  }}>
                    {editingMercadoId === m.id ? (
                      <div style={{ display: 'flex', gap: '8px', width: '100%' }}>
                        <input
                          type="text"
                          value={editingMercadoNome}
                          onChange={(e) => setEditingMercadoNome(e.target.value)}
                          className="form-control"
                          style={{ flex: 1, height: '32px', padding: '0 8px', fontSize: '13px' }}
                        />
                        <button 
                          onClick={() => handleUpdateMercado(m.id, m.idRemote)} 
                          className="btn btn-primary" 
                          style={{ height: '32px', padding: '0 10px', borderRadius: 'var(--radius-sm)' }}
                        >
                          <Check size={14} />
                        </button>
                        <button 
                          onClick={() => setEditingMercadoId(null)} 
                          className="btn btn-secondary" 
                          style={{ height: '32px', padding: '0 10px', borderRadius: 'var(--radius-sm)' }}
                        >
                          <X size={14} />
                        </button>
                      </div>
                    ) : (
                      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
                        <div 
                          style={{ display: 'flex', flexDirection: 'column', cursor: 'pointer', flex: 1 }}
                          onClick={() => setSelectedMercado(m)}
                        >
                          <span style={{ fontWeight: 600, color: isSelected ? 'var(--primary)' : 'var(--text-heading)', fontSize: '15px' }}>
                            {m.nome}
                          </span>
                          {m.syncStatus !== 'synced' && (
                            <span style={{ fontSize: '10px', color: 'var(--warning)', fontWeight: 500 }}>
                              Aguardando sincronização
                            </span>
                          )}
                        </div>
                        <div style={{ display: 'flex', alignItems: 'center', gap: '6px' }}>
                          <button
                            onClick={() => {
                              setEditingMercadoId(m.id);
                              setEditingMercadoNome(m.nome);
                            }}
                            className="btn-text"
                            style={{ padding: '4px', color: 'var(--text-muted)' }}
                            title="Editar"
                          >
                            <Edit2 size={16} />
                          </button>
                          <button
                            onClick={() => handleDeleteMercado(m.id, m.idRemote)}
                            className="btn-text"
                            style={{ padding: '4px', color: 'var(--error)' }}
                            title="Excluir"
                          >
                            <Trash2 size={16} />
                          </button>
                          <button
                            onClick={() => setSelectedMercado(m)}
                            className="btn-text"
                            style={{ padding: '4px 8px', color: 'var(--primary)', display: 'flex', alignItems: 'center', gap: '4px', fontSize: '12px', fontWeight: 600 }}
                          >
                            Setores <ArrowRight size={14} />
                          </button>
                        </div>
                      </div>
                    )}
                  </div>
                );
              })
            )}
          </div>
        </div>

        {/* Bloco de Setores (Só aparece se mercado selecionado) */}
        {selectedMercado ? (
          <div className="glass animate-slide-up" style={{ padding: '20px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)', border: '1px dashed var(--primary)' }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', marginBottom: '16px' }}>
              <h3 style={{ fontSize: '16px', fontWeight: 600, color: 'var(--text-heading)', display: 'flex', alignItems: 'center', gap: '8px', margin: 0 }}>
                <Layers size={18} style={{ color: 'var(--primary)' }} /> Setores de: {selectedMercado.nome}
              </h3>
              <button 
                onClick={() => setSelectedMercado(null)} 
                className="btn-text" 
                style={{ padding: '4px 8px', fontSize: '12px', display: 'flex', alignItems: 'center', gap: '4px', color: 'var(--text-muted)' }}
              >
                <X size={14} /> Fechar
              </button>
            </div>

            {/* Form Setores */}
            <form onSubmit={handleCreateSetor} style={{ display: 'flex', gap: '8px', marginBottom: '16px' }}>
              <input
                type="text"
                required
                placeholder="Ex: Frios, Bebidas, Limpeza..."
                value={setorNome}
                onChange={(e) => setSetorNome(e.target.value)}
                className="form-control"
                style={{ flex: 1, height: '40px', padding: '0 12px', fontSize: '14px' }}
              />
              <button type="submit" disabled={loading} className="btn btn-primary" style={{ height: '40px', padding: '0 14px', borderRadius: 'var(--radius-md)' }}>
                <Plus size={18} />
                Adicionar Setor
              </button>
            </form>

            {/* Listagem de Setores */}
            <div style={{ display: 'flex', flexDirection: 'column', gap: '8px' }}>
              {listSetores.length === 0 ? (
                <div style={{ textAlign: 'center', padding: '20px 10px', color: 'var(--text-muted)', fontSize: '13px' }}>
                  Nenhum setor cadastrado para este mercado.
                </div>
              ) : (
                listSetores.map((s) => (
                  <div key={s.id} style={{
                    display: 'flex',
                    alignItems: 'center',
                    justifyContent: 'space-between',
                    padding: '10px 12px',
                    borderRadius: 'var(--radius-sm)',
                    border: '1px solid var(--border-color)',
                    backgroundColor: 'rgba(255, 255, 255, 0.01)'
                  }}>
                    {editingSetorId === s.id ? (
                      <div style={{ display: 'flex', gap: '8px', width: '100%' }}>
                        <input
                          type="text"
                          value={editingSetorNome}
                          onChange={(e) => setEditingSetorNome(e.target.value)}
                          className="form-control"
                          style={{ flex: 1, height: '32px', padding: '0 8px', fontSize: '13px' }}
                        />
                        <button 
                          onClick={() => handleUpdateSetor(s.id, s.idRemote)} 
                          className="btn btn-primary" 
                          style={{ height: '32px', padding: '0 10px', borderRadius: 'var(--radius-sm)' }}
                        >
                          <Check size={14} />
                        </button>
                        <button 
                          onClick={() => setEditingSetorId(null)} 
                          className="btn btn-secondary" 
                          style={{ height: '32px', padding: '0 10px', borderRadius: 'var(--radius-sm)' }}
                        >
                          <X size={14} />
                        </button>
                      </div>
                    ) : (
                      <>
                        <div style={{ display: 'flex', flexDirection: 'column' }}>
                          <span style={{ fontWeight: 500, color: 'var(--text-heading)', fontSize: '14px' }}>
                            {s.nome}
                          </span>
                          {s.syncStatus !== 'synced' && (
                            <span style={{ fontSize: '10px', color: 'var(--warning)', fontWeight: 500 }}>
                              Aguardando sincronização
                            </span>
                          )}
                        </div>
                        <div style={{ display: 'flex', gap: '6px' }}>
                          <button
                            onClick={() => {
                              setEditingSetorId(s.id);
                              setEditingSetorNome(s.nome);
                            }}
                            className="btn-text"
                            style={{ padding: '4px', color: 'var(--text-muted)' }}
                            title="Editar"
                          >
                            <Edit2 size={14} />
                          </button>
                          <button
                            onClick={() => handleDeleteSetor(s.id, s.idRemote)}
                            className="btn-text"
                            style={{ padding: '4px', color: 'var(--error)' }}
                            title="Excluir"
                          >
                            <Trash2 size={14} />
                          </button>
                        </div>
                      </>
                    )}
                  </div>
                ))
              )}
            </div>
          </div>
        ) : (
          <div style={{
            textAlign: 'center',
            padding: '30px 20px',
            border: '2px dashed var(--border-color)',
            borderRadius: 'var(--radius-md)',
            color: 'var(--text-muted)',
            fontSize: '14px'
          }}>
            <Tag size={24} style={{ marginBottom: '8px', opacity: 0.5 }} />
            <p style={{ margin: 0 }}>Selecione um Mercado acima para ver e gerenciar seus setores.</p>
          </div>
        )}
      </div>
    </div>
  );
}
