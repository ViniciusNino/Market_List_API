import React, { useState, useEffect } from 'react';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { Plus, Trash2, Edit2, Check, X, ShoppingBag, Filter } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function Items() {
  const [nome, setNome] = useState('');
  const [unidadeMedida, setUnidadeMedida] = useState('UN');
  const [unidadeId, setUnidadeId] = useState(''); // Ambiente
  const [sessaoId, setSessaoId] = useState('');   // Mercado
  const [setorId, setSetorId] = useState('');     // Setor
  
  const [editingId, setEditingId] = useState(null);
  const [editingNome, setEditingNome] = useState('');
  const [editingUnidadeMedida, setEditingUnidadeMedida] = useState('UN');
  const [editingUnidadeId, setEditingUnidadeId] = useState('');
  const [editingSessaoId, setEditingSessaoId] = useState('');
  const [editingSetorId, setEditingSetorId] = useState('');

  // Filtros de visualização
  const [filterUnidadeId, setFilterUnidadeId] = useState('all');
  const [filterSessaoId, setFilterSessaoId] = useState('all');

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const online = useStore((state) => state.online);

  // Consultas reativas do Dexie
  const locaisItens = useLiveQuery(() => db.itens.toArray());
  const locaisAmbientes = useLiveQuery(() => db.ambientes.toArray());
  const locaisMercados = useLiveQuery(() => db.mercados.toArray());
  const locaisSetores = useLiveQuery(() => db.setores.toArray());

  // Carregar itens da API e salvar no Dexie
  const loadItems = async () => {
    if (!online) return;
    try {
      setLoading(true);
      const response = await api.get('/Item');
      
      await db.transaction('rw', db.itens, async () => {
        await db.itens.clear();
        const records = response.map(item => ({
          idRemote: item.Id,
          nome: item.SNome,
          unidadeMedida: item.SUnidadeMedida,
          unidadeId: item.NIdUnidade,
          sessaoId: item.NIdSessao,
          setorId: item.NIdSetor,
          syncStatus: 'synced'
        }));
        await db.itens.bulkAdd(records);
      });
    } catch (err) {
      console.error("Erro ao carregar itens da API:", err);
      setError('Erro ao sincronizar itens do servidor. Exibindo dados offline.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadItems();
  }, [online]);

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!nome.trim() || !unidadeId || !sessaoId) {
      setError('Por favor, preencha o Nome, o Ambiente e o Mercado.');
      return;
    }
    setLoading(true);
    setError('');

    const targetUnidadeId = parseInt(unidadeId);
    const targetSessaoId = parseInt(sessaoId);
    const targetSetorId = setorId ? parseInt(setorId) : null;

    // Buscar IDs remotos correspondentes para enviar para a API se online
    let remoteUnidadeId = targetUnidadeId;
    let remoteSessaoId = targetSessaoId;
    let remoteSetorId = targetSetorId;

    if (online) {
      const dbAmb = await db.ambientes.get(targetUnidadeId);
      const dbMerc = await db.mercados.get(targetSessaoId);
      const dbSet = targetSetorId ? await db.setores.get(targetSetorId) : null;

      remoteUnidadeId = dbAmb?.idRemote || targetUnidadeId;
      remoteSessaoId = dbMerc?.idRemote || targetSessaoId;
      remoteSetorId = dbSet?.idRemote || targetSetorId;
    }

    try {
      if (online && remoteUnidadeId && remoteSessaoId) {
        await api.post('/Item', {
          sNome: nome.trim(),
          sUnidadeMedida: unidadeMedida,
          nIdUnidade: remoteUnidadeId,
          nIdSessao: remoteSessaoId,
          nIdSetor: remoteSetorId
        });
        await loadItems();
      } else {
        // Adiciona offline
        await db.itens.add({
          nome: nome.trim(),
          unidadeMedida,
          unidadeId: targetUnidadeId,
          sessaoId: targetSessaoId,
          setorId: targetSetorId,
          syncStatus: 'created'
        });
      }
      setNome('');
      setUnidadeMedida('UN');
      setUnidadeId('');
      setSessaoId('');
      setSetorId('');
    } catch (err) {
      setError(err.message || 'Erro ao criar item.');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async (id, idRemote) => {
    if (!editingNome.trim() || !editingUnidadeId || !editingSessaoId) return;
    setLoading(true);
    setError('');

    const targetUnidadeId = parseInt(editingUnidadeId);
    const targetSessaoId = parseInt(editingSessaoId);
    const targetSetorId = editingSetorId ? parseInt(editingSetorId) : null;

    let remoteUnidadeId = targetUnidadeId;
    let remoteSessaoId = targetSessaoId;
    let remoteSetorId = targetSetorId;

    if (online) {
      const dbAmb = await db.ambientes.get(targetUnidadeId);
      const dbMerc = await db.mercados.get(targetSessaoId);
      const dbSet = targetSetorId ? await db.setores.get(targetSetorId) : null;

      remoteUnidadeId = dbAmb?.idRemote || targetUnidadeId;
      remoteSessaoId = dbMerc?.idRemote || targetSessaoId;
      remoteSetorId = dbSet?.idRemote || targetSetorId;
    }

    try {
      if (online && idRemote && remoteUnidadeId && remoteSessaoId) {
        await api.put(`/Item/${idRemote}`, {
          id: idRemote,
          sNome: editingNome.trim(),
          sUnidadeMedida: editingUnidadeMedida,
          nIdUnidade: remoteUnidadeId,
          nIdSessao: remoteSessaoId,
          nIdSetor: remoteSetorId
        });
        await db.itens.update(id, {
          nome: editingNome.trim(),
          unidadeMedida: editingUnidadeMedida,
          unidadeId: targetUnidadeId,
          sessaoId: targetSessaoId,
          setorId: targetSetorId,
          syncStatus: 'synced'
        });
      } else {
        const item = await db.itens.get(id);
        const status = item.syncStatus === 'created' ? 'created' : 'updated';
        await db.itens.update(id, {
          nome: editingNome.trim(),
          unidadeMedida: editingUnidadeMedida,
          unidadeId: targetUnidadeId,
          sessaoId: targetSessaoId,
          setorId: targetSetorId,
          syncStatus: status
        });
      }
      setEditingId(null);
    } catch (err) {
      setError(err.message || 'Erro ao atualizar item.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id, idRemote) => {
    if (!window.confirm('Deseja realmente remover este item?')) return;
    setLoading(true);
    setError('');

    try {
      if (online && idRemote) {
        await api.delete(`/Item/${idRemote}`);
        await db.itens.delete(id);
      } else {
        const item = await db.itens.get(id);
        if (item.syncStatus === 'created') {
          await db.itens.delete(id);
        } else {
          await db.itens.update(id, { syncStatus: 'deleted' });
        }
      }
    } catch (err) {
      setError(err.message || 'Erro ao deletar item.');
    } finally {
      setLoading(false);
    }
  };

  // Listagens seguras
  const activeAmbientes = locaisAmbientes ? locaisAmbientes.filter(a => a.syncStatus !== 'deleted') : [];
  const activeMercados = locaisMercados ? locaisMercados.filter(m => m.syncStatus !== 'deleted') : [];
  
  // Setores disponíveis para cadastro com base no Mercado selecionado
  const activeSetores = (locaisSetores && sessaoId) 
    ? locaisSetores.filter(s => s.sessaoId === parseInt(sessaoId) && s.syncStatus !== 'deleted') 
    : [];

  const activeSetoresEdit = (locaisSetores && editingSessaoId)
    ? locaisSetores.filter(s => s.sessaoId === parseInt(editingSessaoId) && s.syncStatus !== 'deleted')
    : [];

  // Filtragem dos itens exibidos
  const listItens = locaisItens ? locaisItens.filter(item => {
    if (item.syncStatus === 'deleted') return false;
    
    if (filterUnidadeId !== 'all' && item.unidadeId !== parseInt(filterUnidadeId)) return false;
    if (filterSessaoId !== 'all' && item.sessaoId !== parseInt(filterSessaoId)) return false;
    
    return true;
  }) : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
      {/* Header */}
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
          <ShoppingBag size={22} />
        </div>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '22px' }}>
            Itens / Catálogo
          </h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>
            Gerencie o cadastro de produtos, marcas e unidades de medida
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

      {/* Formulário de Cadastro */}
      <div className="glass" style={{ padding: '20px', borderRadius: 'var(--radius-md)', marginBottom: '24px', boxShadow: 'var(--shadow-sm)' }}>
        <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '16px' }}>
          Adicionar Novo Item ao Catálogo
        </h3>
        <form onSubmit={handleCreate} style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
          
          <div style={{ display: 'flex', gap: '10px' }}>
            <div style={{ flex: 1 }}>
              <input
                type="text"
                required
                placeholder="Nome do item (ex: Pão de Forma)"
                value={nome}
                onChange={(e) => setNome(e.target.value)}
                className="form-control"
                style={{ height: '40px', fontSize: '14px' }}
              />
            </div>
            <div style={{ width: '90px' }}>
              <select
                value={unidadeMedida}
                onChange={(e) => setUnidadeMedida(e.target.value)}
                className="form-control"
                style={{ height: '40px', fontSize: '14px', padding: '0 8px' }}
              >
                <option value="UN">UN</option>
                <option value="KG">KG</option>
                <option value="LT">LT</option>
                <option value="CX">CX</option>
                <option value="PCT">PCT</option>
                <option value="G">G</option>
                <option value="ML">ML</option>
              </select>
            </div>
          </div>

          <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '10px' }}>
            <div>
              <label className="form-label" style={{ marginBottom: '4px', fontSize: '12px' }}>Ambiente (Origem)</label>
              <select
                required
                value={unidadeId}
                onChange={(e) => setUnidadeId(e.target.value)}
                className="form-control"
                style={{ height: '40px', fontSize: '13px', padding: '0 8px' }}
              >
                <option value="">Selecione...</option>
                {activeAmbientes.map(a => (
                  <option key={a.id} value={a.id}>{a.nome}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="form-label" style={{ marginBottom: '4px', fontSize: '12px' }}>Mercado (Onde comprar)</label>
              <select
                required
                value={sessaoId}
                onChange={(e) => {
                  setSessaoId(e.target.value);
                  setSetorId(''); // reseta setor ao mudar mercado
                }}
                className="form-control"
                style={{ height: '40px', fontSize: '13px', padding: '0 8px' }}
              >
                <option value="">Selecione...</option>
                {activeMercados.map(m => (
                  <option key={m.id} value={m.id}>{m.nome}</option>
                ))}
              </select>
            </div>
          </div>

          <div>
            <label className="form-label" style={{ marginBottom: '4px', fontSize: '12px' }}>Setor / Corredor (Opcional)</label>
            <select
              value={setorId}
              onChange={(e) => setSetorId(e.target.value)}
              disabled={!sessaoId}
              className="form-control"
              style={{ height: '40px', fontSize: '13px', padding: '0 8px' }}
            >
              <option value="">Nenhum</option>
              {activeSetores.map(s => (
                <option key={s.id} value={s.id}>{s.nome}</option>
              ))}
            </select>
          </div>

          <button type="submit" disabled={loading} className="btn btn-primary btn-full" style={{ height: '40px', marginTop: '4px' }}>
            <Plus size={18} />
            Cadastrar Item
          </button>
        </form>
      </div>

      {/* Filtros da Listagem */}
      <div className="glass" style={{ padding: '16px', borderRadius: 'var(--radius-md)', marginBottom: '16px', boxShadow: 'var(--shadow-sm)' }}>
        <h4 style={{ fontSize: '13px', fontWeight: 600, color: 'var(--text-muted)', marginBottom: '12px', display: 'flex', alignItems: 'center', gap: '6px' }}>
          <Filter size={14} /> Filtros de Visualização
        </h4>
        <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '10px' }}>
          <div>
            <select
              value={filterUnidadeId}
              onChange={(e) => setFilterUnidadeId(e.target.value)}
              className="form-control"
              style={{ height: '36px', fontSize: '12px', padding: '0 8px' }}
            >
              <option value="all">Todos os Ambientes</option>
              {activeAmbientes.map(a => (
                <option key={a.id} value={a.id}>{a.nome}</option>
              ))}
            </select>
          </div>
          <div>
            <select
              value={filterSessaoId}
              onChange={(e) => setFilterSessaoId(e.target.value)}
              className="form-control"
              style={{ height: '36px', fontSize: '12px', padding: '0 8px' }}
            >
              <option value="all">Todos os Mercados</option>
              {activeMercados.map(m => (
                <option key={m.id} value={m.id}>{m.nome}</option>
              ))}
            </select>
          </div>
        </div>
      </div>

      {/* Listagem */}
      <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
        {listItens.length === 0 ? (
          <div style={{ textAlign: 'center', padding: '40px 20px', color: 'var(--text-muted)' }}>
            Nenhum item encontrado no catálogo.
          </div>
        ) : (
          listItens.map((item) => {
            const ambienteObj = activeAmbientes.find(a => a.id === item.unidadeId || (a.idRemote && a.idRemote === item.unidadeId));
            const mercadoObj = activeMercados.find(m => m.id === item.sessaoId || (m.idRemote && m.idRemote === item.sessaoId));
            const setorObj = locaisSetores ? locaisSetores.find(s => s.id === item.setorId || (s.idRemote && s.idRemote === item.setorId)) : null;

            return (
              <div key={item.id} className="glass" style={{
                padding: '16px',
                borderRadius: 'var(--radius-md)',
                boxShadow: 'var(--shadow-sm)',
                display: 'flex',
                flexDirection: 'column',
                gap: '8px'
              }}>
                {editingId === item.id ? (
                  <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
                    <div style={{ display: 'flex', gap: '8px' }}>
                      <input
                        type="text"
                        value={editingNome}
                        onChange={(e) => setEditingNome(e.target.value)}
                        className="form-control"
                        style={{ flex: 1, height: '36px', padding: '0 8px', fontSize: '13px' }}
                      />
                      <select
                        value={editingUnidadeMedida}
                        onChange={(e) => setEditingUnidadeMedida(e.target.value)}
                        className="form-control"
                        style={{ width: '80px', height: '36px', padding: '0 4px', fontSize: '13px' }}
                      >
                        <option value="UN">UN</option>
                        <option value="KG">KG</option>
                        <option value="LT">LT</option>
                        <option value="CX">CX</option>
                        <option value="PCT">PCT</option>
                      </select>
                    </div>

                    <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '8px' }}>
                      <select
                        value={editingUnidadeId}
                        onChange={(e) => setEditingUnidadeId(e.target.value)}
                        className="form-control"
                        style={{ height: '36px', fontSize: '12px', padding: '0 4px' }}
                      >
                        <option value="">Ambiente...</option>
                        {activeAmbientes.map(a => (
                          <option key={a.id} value={a.id}>{a.nome}</option>
                        ))}
                      </select>
                      <select
                        value={editingSessaoId}
                        onChange={(e) => {
                          setEditingSessaoId(e.target.value);
                          setEditingSetorId('');
                        }}
                        className="form-control"
                        style={{ height: '36px', fontSize: '12px', padding: '0 4px' }}
                      >
                        <option value="">Mercado...</option>
                        {activeMercados.map(m => (
                          <option key={m.id} value={m.id}>{m.nome}</option>
                        ))}
                      </select>
                    </div>

                    <select
                      value={editingSetorId}
                      onChange={(e) => setEditingSetorId(e.target.value)}
                      disabled={!editingSessaoId}
                      className="form-control"
                      style={{ height: '36px', fontSize: '12px', padding: '0 4px' }}
                    >
                      <option value="">Nenhum Setor</option>
                      {activeSetoresEdit.map(s => (
                        <option key={s.id} value={s.id}>{s.nome}</option>
                      ))}
                    </select>

                    <div style={{ display: 'flex', justifyContent: 'flex-end', gap: '8px' }}>
                      <button 
                        onClick={() => handleUpdate(item.id, item.idRemote)} 
                        className="btn btn-primary" 
                        style={{ height: '32px', padding: '0 12px', borderRadius: 'var(--radius-sm)', fontSize: '12px' }}
                      >
                        <Check size={14} /> Salvar
                      </button>
                      <button 
                        onClick={() => setEditingId(null)} 
                        className="btn btn-secondary" 
                        style={{ height: '32px', padding: '0 12px', borderRadius: 'var(--radius-sm)', fontSize: '12px' }}
                      >
                        <X size={14} /> Cancelar
                      </button>
                    </div>
                  </div>
                ) : (
                  <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'flex-start' }}>
                    <div style={{ display: 'flex', flexDirection: 'column', gap: '4px' }}>
                      <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
                        <span style={{ fontWeight: 600, color: 'var(--text-heading)', fontSize: '15px' }}>
                          {item.nome}
                        </span>
                        <span style={{
                          fontSize: '10px',
                          fontWeight: 700,
                          backgroundColor: 'var(--primary-light)',
                          color: 'var(--primary)',
                          padding: '2px 6px',
                          borderRadius: 'var(--radius-sm)'
                        }}>
                          {item.unidadeMedida || 'UN'}
                        </span>
                      </div>

                      {/* Info Mapeada */}
                      <div style={{ display: 'flex', flexWrap: 'wrap', gap: '6px', fontSize: '11px', color: 'var(--text-muted)' }}>
                        <span style={{ border: '1px solid var(--border-color)', padding: '2px 6px', borderRadius: 'var(--radius-sm)' }}>
                          Ambiente: <strong>{ambienteObj?.nome || 'Desconhecido'}</strong>
                        </span>
                        <span style={{ border: '1px solid var(--border-color)', padding: '2px 6px', borderRadius: 'var(--radius-sm)' }}>
                          Mercado: <strong>{mercadoObj?.nome || 'Desconhecido'}</strong>
                        </span>
                        {setorObj && (
                          <span style={{ border: '1px solid var(--border-color)', padding: '2px 6px', borderRadius: 'var(--radius-sm)' }}>
                            Setor: <strong>{setorObj.nome}</strong>
                          </span>
                        )}
                      </div>

                      {item.syncStatus !== 'synced' && (
                        <span style={{ fontSize: '10px', color: 'var(--warning)', fontWeight: 500 }}>
                          Aguardando sincronização ({item.syncStatus})
                        </span>
                      )}
                    </div>

                    <div style={{ display: 'flex', gap: '4px' }}>
                      <button
                        onClick={() => {
                          setEditingId(item.id);
                          setEditingNome(item.nome);
                          setEditingUnidadeMedida(item.unidadeMedida || 'UN');
                          setEditingUnidadeId(item.unidadeId || '');
                          setEditingSessaoId(item.sessaoId || '');
                          setEditingSetorId(item.setorId || '');
                        }}
                        className="btn-text"
                        style={{ padding: '6px', color: 'var(--text-muted)' }}
                        title="Editar"
                      >
                        <Edit2 size={16} />
                      </button>
                      <button
                        onClick={() => handleDelete(item.id, item.idRemote)}
                        className="btn-text"
                        style={{ padding: '6px', color: 'var(--error)' }}
                        title="Excluir"
                      >
                        <Trash2 size={16} />
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
  );
}
