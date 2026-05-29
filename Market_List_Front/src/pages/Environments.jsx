import React, { useState, useEffect } from 'react';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { Plus, Trash2, Edit2, Check, X, MapPin } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function Environments() {
  const [nome, setNome] = useState('');
  const [editingId, setEditingId] = useState(null);
  const [editingNome, setEditingNome] = useState('');
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const online = useStore((state) => state.online);
  const user = useStore((state) => state.user);

  // Consulta reativa do Dexie local
  const locais = useLiveQuery(() => db.ambientes.toArray());

  // Carregar ambientes
  const loadAmbientes = async () => {
    if (!online) return;
    try {
      const response = await api.get(`/Unidade/Unidades/${user.Id}`);
      // Salva no Dexie limpando o cache anterior
      await db.transaction('rw', db.ambientes, async () => {
        await db.ambientes.clear();
        const records = response.map(item => ({
          idRemote: item.Id,
          nome: item.Nome,
          syncStatus: 'synced'
        }));
        await db.ambientes.bulkAdd(records);
      });
    } catch (err) {
      console.error("Erro ao sincronizar ambientes do backend:", err);
    }
  };

  useEffect(() => {
    loadAmbientes();
  }, [online]);

  const handleCreate = async (e) => {
    e.preventDefault();
    if (!nome.trim()) return;
    setLoading(true);
    setError('');

    try {
      if (online) {
        // Envia para o backend
        const response = await api.post('/Unidade/Unidades', {
          nome: nome.trim(),
          idUsuario: user.Id
        });
        
        // Recarrega da API para obter o novo ID remoto
        await loadAmbientes();
      } else {
        // Salva offline no Dexie
        await db.ambientes.add({
          nome: nome.trim(),
          syncStatus: 'created'
        });
      }
      setNome('');
    } catch (err) {
      setError(err.message || 'Erro ao criar ambiente.');
    } finally {
      setLoading(false);
    }
  };

  const handleUpdate = async (id, idRemote) => {
    if (!editingNome.trim()) return;
    setLoading(true);

    try {
      if (online && idRemote) {
        await api.put(`/Unidade/Unidades/${idRemote}`, {
          id: idRemote,
          nome: editingNome.trim(),
          idUsuario: user.Id
        });
        
        await db.ambientes.update(id, { nome: editingNome.trim(), syncStatus: 'synced' });
      } else {
        const item = await db.ambientes.get(id);
        const status = item.syncStatus === 'created' ? 'created' : 'updated';
        await db.ambientes.update(id, { nome: editingNome.trim(), syncStatus: status });
      }
      setEditingId(null);
    } catch (err) {
      setError(err.message || 'Erro ao editar ambiente.');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id, idRemote) => {
    if (!window.confirm('Deseja realmente remover este ambiente? Todas as listas vinculadas a ele serão afetadas.')) return;
    setLoading(true);

    try {
      if (online && idRemote) {
        await api.delete(`/Unidade/Unidades/${idRemote}`);
        await db.ambientes.delete(id);
      } else {
        const item = await db.ambientes.get(id);
        if (item.syncStatus === 'created') {
          // Se só existe offline, deletamos direto
          await db.ambientes.delete(id);
        } else {
          // Se já está no servidor, marcamos como deletado para sincronizar depois
          await db.ambientes.update(id, { syncStatus: 'deleted' });
        }
      }
    } catch (err) {
      setError(err.message || 'Erro ao deletar ambiente.');
    } finally {
      setLoading(false);
    }
  };

  // Filtra ambientes ativos (não marcados como deletados offline)
  const listAmbientes = locais ? locais.filter(a => a.syncStatus !== 'deleted') : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
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
          <MapPin size={22} />
        </div>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '22px' }}>Ambientes</h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>Origens das listas (Residências, Lojas, etc)</p>
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

      {/* Form Criar */}
      <form onSubmit={handleCreate} style={{ display: 'flex', gap: '10px', marginBottom: '24px' }}>
        <input
          type="text"
          required
          placeholder="Ex: Loja 1, Loja 2, Casa..."
          value={nome}
          onChange={(e) => setNome(e.target.value)}
          className="form-control"
          style={{ flex: 1, height: '44px', padding: '10px 16px' }}
        />
        <button type="submit" disabled={loading} className="btn btn-primary" style={{ height: '44px', padding: '0 16px' }}>
          <Plus size={20} />
          Criar
        </button>
      </form>

      {/* Listagem */}
      <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
        {listAmbientes.length === 0 ? (
          <div style={{ textAlign: 'center', padding: '40px 20px', color: 'var(--text-muted)' }}>
            Nenhum ambiente cadastrado.
          </div>
        ) : (
          listAmbientes.map((ambiente) => (
            <div key={ambiente.id} className="glass" style={{
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'space-between',
              padding: '16px',
              borderRadius: 'var(--radius-md)',
              boxShadow: 'var(--shadow-sm)'
            }}>
              {editingId === ambiente.id ? (
                <div style={{ display: 'flex', gap: '8px', width: '100%' }}>
                  <input
                    type="text"
                    value={editingNome}
                    onChange={(e) => setEditingNome(e.target.value)}
                    className="form-control"
                    style={{ flex: 1, height: '36px', padding: '0 12px' }}
                  />
                  <button 
                    onClick={() => handleUpdate(ambiente.id, ambiente.idRemote)} 
                    className="btn btn-primary" 
                    style={{ height: '36px', padding: '0 12px', borderRadius: 'var(--radius-sm)' }}
                  >
                    <Check size={16} />
                  </button>
                  <button 
                    onClick={() => setEditingId(null)} 
                    className="btn btn-secondary" 
                    style={{ height: '36px', padding: '0 12px', borderRadius: 'var(--radius-sm)' }}
                  >
                    <X size={16} />
                  </button>
                </div>
              ) : (
                <>
                  <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-start' }}>
                    <span style={{ fontWeight: 600, color: 'var(--text-heading)', fontSize: '16px' }}>
                      {ambiente.nome}
                    </span>
                    {ambiente.syncStatus !== 'synced' && (
                      <span style={{ fontSize: '11px', color: 'var(--warning)', fontWeight: 500 }}>
                        {ambiente.syncStatus === 'created' ? 'Aguardando sincronização (Criar)' : 'Aguardando sincronização (Editar)'}
                      </span>
                    )}
                  </div>
                  <div style={{ display: 'flex', gap: '8px' }}>
                    <button
                      onClick={() => {
                        setEditingId(ambiente.id);
                        setEditingNome(ambiente.nome);
                      }}
                      className="btn-text"
                      style={{ padding: '6px', color: 'var(--text-muted)', borderRadius: 'var(--radius-sm)' }}
                      title="Editar"
                    >
                      <Edit2 size={18} />
                    </button>
                    <button
                      onClick={() => handleDelete(ambiente.id, ambiente.idRemote)}
                      className="btn-text"
                      style={{ padding: '6px', color: 'var(--error)', borderRadius: 'var(--radius-sm)' }}
                      title="Excluir"
                    >
                      <Trash2 size={18} />
                    </button>
                  </div>
                </>
              )}
            </div>
          ))
        )}
      </div>
    </div>
  );
}
