import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { db } from '../db/db';
import { api } from '../services/api';
import { useStore } from '../store/useStore';
import { ArrowLeft, Layers, Check, AlertCircle } from 'lucide-react';
import { useLiveQuery } from 'dexie-react-hooks';

export default function MergeLists() {
  const navigate = useNavigate();
  const [descricao, setDescricao] = useState('');
  const [selectedListIds, setSelectedListIds] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState(false);

  const online = useStore((state) => state.online);
  const user = useStore((state) => state.user);

  // Queries reativas locais do Dexie
  const locaisListas = useLiveQuery(() => db.listas.toArray());
  const locaisAmbientes = useLiveQuery(() => db.ambientes.toArray());

  const handleToggleList = (id) => {
    if (selectedListIds.includes(id)) {
      setSelectedListIds(selectedListIds.filter(item => item !== id));
    } else {
      setSelectedListIds([...selectedListIds, id]);
    }
  };

  const handleMerge = async (e) => {
    e.preventDefault();
    if (selectedListIds.length < 2) {
      setError('Por favor, selecione pelo menos duas listas para unificar.');
      return;
    }
    if (!descricao.trim()) {
      setError('Informe um nome descritivo para a Lista Mestra.');
      return;
    }

    setLoading(true);
    setError('');
    setSuccess(false);

    try {
      if (online) {
        // Encontra os IDs remotos correspondentes para enviar à API
        const lists = await db.listas.toArray();
        const remoteIds = selectedListIds.map(localId => {
          const list = lists.find(l => l.id === localId);
          return list ? list.idRemote : null;
        }).filter(id => id !== null);

        if (remoteIds.length < 2) {
          throw new Error("Algumas das listas selecionadas ainda não foram sincronizadas com o servidor. Aguarde a sincronização ou realize a união offline.");
        }

        const payload = {
          usuarioId: user.Id,
          listaIds: remoteIds,
          status: 1, // Status Ativo do Agrupador
          descricao: descricao.trim()
        };

        // POST /Lista/Agrupar
        await api.post('/Lista/Agrupar', payload);
        setSuccess(true);
        setTimeout(() => {
          navigate('/');
        }, 1500);
      } else {
        // Fluxo Offline: Não cria agrupador na API, mas inicia o modo compras unificado imediatamente
        // passamos a lista de IDs locais no state da navegação
        navigate('/shopping/merged', { 
          state: { 
            localListIds: selectedListIds,
            nomeMestra: descricao.trim()
          } 
        });
      }
    } catch (err) {
      console.error(err);
      setError(err.message || 'Erro ao realizar união das listas.');
    } finally {
      setLoading(false);
    }
  };

  // Filtragem das listas válidas
  const activeListas = locaisListas ? locaisListas.filter(l => l.syncStatus !== 'deleted') : [];
  const activeAmbientes = locaisAmbientes ? locaisAmbientes.filter(a => a.syncStatus !== 'deleted') : [];

  return (
    <div style={{ padding: '20px' }} className="animate-fade-in">
      
      {/* Voltar e Header */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginBottom: '24px' }}>
        <button 
          onClick={() => navigate('/')} 
          className="btn-text" 
          style={{ padding: '8px', borderRadius: 'var(--radius-sm)', display: 'inline-flex', color: 'var(--text-heading)' }}
        >
          <ArrowLeft size={20} />
        </button>
        <div>
          <h2 style={{ color: 'var(--text-heading)', fontWeight: 700, margin: 0, fontSize: '20px' }}>Unificar Listas</h2>
          <p style={{ color: 'var(--text-muted)', fontSize: '13px', margin: 0 }}>Consolide listas de múltiplos estabelecimentos</p>
        </div>
      </div>

      {error && (
        <div style={{
          padding: '12px 16px',
          borderRadius: 'var(--radius-sm)',
          backgroundColor: 'rgba(239, 68, 68, 0.1)',
          color: 'var(--error)',
          fontSize: '14px',
          marginBottom: '20px',
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}>
          <AlertCircle size={18} />
          {error}
        </div>
      )}

      {success && (
        <div style={{
          padding: '12px 16px',
          borderRadius: 'var(--radius-sm)',
          backgroundColor: 'rgba(16, 185, 129, 0.1)',
          color: 'var(--success)',
          fontSize: '14px',
          marginBottom: '20px',
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}>
          <Check size={18} />
          Unificação realizada com sucesso! Redirecionando...
        </div>
      )}

      {/* Formulário de União */}
      <form onSubmit={handleMerge} style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
        
        {/* Bloco 1: Nome da Lista Mestra */}
        <div className="glass" style={{ padding: '20px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
          <label className="form-label" style={{ marginBottom: '8px' }}>Nome da Lista Mestra Unificada</label>
          <input
            type="text"
            required
            placeholder="Ex: Compras no Ceasa de Sexta"
            value={descricao}
            onChange={(e) => setDescricao(e.target.value)}
            className="form-control"
            style={{ height: '44px' }}
          />
        </div>

        {/* Bloco 2: Seleção das Listas */}
        <div className="glass" style={{ padding: '20px', borderRadius: 'var(--radius-md)', boxShadow: 'var(--shadow-sm)' }}>
          <h3 style={{ fontSize: '15px', fontWeight: 600, color: 'var(--text-heading)', marginBottom: '14px', display: 'flex', alignItems: 'center', gap: '8px' }}>
            <Layers size={16} style={{ color: 'var(--primary)' }} /> Selecione as listas para unir
          </h3>

          <div style={{ display: 'flex', flexDirection: 'column', gap: '10px' }}>
            {activeListas.length === 0 ? (
              <div style={{ textAlign: 'center', padding: '20px 0', color: 'var(--text-muted)', fontSize: '13px' }}>
                Nenhuma lista disponível para unificar.
              </div>
            ) : (
              activeListas.map((lista) => {
                const amb = activeAmbientes.find(a => a.id === lista.unidadeId);
                const isChecked = selectedListIds.includes(lista.id);

                return (
                  <div 
                    key={lista.id}
                    onClick={() => handleToggleList(lista.id)}
                    style={{
                      display: 'flex',
                      alignItems: 'center',
                      gap: '12px',
                      padding: '12px 16px',
                      borderRadius: 'var(--radius-sm)',
                      border: isChecked ? '1px solid var(--primary)' : '1px solid var(--border-color)',
                      backgroundColor: isChecked ? 'var(--primary-light)' : 'rgba(255, 255, 255, 0.01)',
                      cursor: 'pointer',
                      transition: 'all var(--transition-fast)'
                    }}
                  >
                    {/* Checkbox Circular */}
                    <div style={{
                      width: '20px',
                      height: '20px',
                      borderRadius: '50%',
                      border: isChecked ? '2px solid var(--primary)' : '2px solid var(--text-light)',
                      display: 'flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      backgroundColor: isChecked ? 'var(--primary)' : 'transparent',
                      transition: 'all var(--transition-fast)'
                    }}>
                      {isChecked && <Check size={12} style={{ color: '#fff' }} />}
                    </div>

                    <div style={{ display: 'flex', flexDirection: 'column' }}>
                      <span style={{ fontWeight: 600, color: 'var(--text-heading)', fontSize: '14px' }}>
                        {lista.nome}
                      </span>
                      <span style={{ fontSize: '11px', color: 'var(--text-muted)' }}>
                        Ambiente: <strong>{amb ? amb.nome : 'Desconhecido'}</strong>
                      </span>
                    </div>
                  </div>
                );
              })
            )}
          </div>
        </div>

        {/* Informações de Conexão */}
        {!online && (
          <div style={{
            fontSize: '12px',
            color: 'var(--warning)',
            backgroundColor: 'rgba(245, 158, 11, 0.1)',
            padding: '10px 14px',
            borderRadius: 'var(--radius-sm)',
            textAlign: 'center'
          }}>
            Você está <strong>offline</strong>. A unificação será processada localmente e as compras serão iniciadas imediatamente.
          </div>
        )}

        <button 
          type="submit" 
          disabled={loading || selectedListIds.length < 2} 
          className="btn btn-primary btn-full"
          style={{ height: '46px' }}
        >
          <Layers size={18} />
          {online ? 'Unificar e Salvar no Servidor' : 'Iniciar Compras Unificadas Offline'}
        </button>

      </form>
    </div>
  );
}
