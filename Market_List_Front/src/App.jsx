import React, { useEffect } from 'react';
import { BrowserRouter, Routes, Route, Navigate, Link, useLocation } from 'react-router-dom';
import { useStore } from './store/useStore';
import Login from './pages/Login';
import Dashboard from './pages/Dashboard';
import Environments from './pages/Environments';
import Markets from './pages/Markets';
import Items from './pages/Items';
import ListDetails from './pages/ListDetails';
import MergeLists from './pages/MergeLists';
import ShoppingMode from './pages/ShoppingMode';
import { ClipboardList, MapPin, Store, ShoppingBag, LogOut, Wifi, WifiOff } from 'lucide-react';

import { runSync } from './services/syncService';

function PrivateRoute({ children }) {
  const token = useStore((state) => state.token);
  return token ? children : <Navigate to="/login" replace />;
}

// Layout com barra de navegação inferior premium
function AppLayout({ children }) {
  const logout = useStore((state) => state.logout);
  const user = useStore((state) => state.user);
  const online = useStore((state) => state.online);
  const location = useLocation();

  const getActiveTab = (path) => {
    if (path === '/' || path.startsWith('/list') || path.startsWith('/merge') || path.startsWith('/shopping')) return 'lists';
    if (path === '/environments') return 'environments';
    if (path === '/markets') return 'markets';
    if (path === '/items') return 'items';
    return '';
  };

  const activeTab = getActiveTab(location.pathname);

  return (
    <div className="app-container">
      {/* Header Fixo */}
      <header className="glass" style={{
        position: 'sticky',
        top: 0,
        zIndex: 100,
        padding: '16px 20px',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        borderBottom: '1px solid var(--border-color)',
        height: '64px'
      }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
          <h1 style={{ fontSize: '20px', margin: 0, fontWeight: 700, color: 'var(--text-heading)' }}>
            Market List
          </h1>
          {online ? (
            <span title="Online" style={{ display: 'inline-flex', color: 'var(--success)' }}>
              <Wifi size={16} />
            </span>
          ) : (
            <span title="Offline - Modo Cache Local" style={{ display: 'inline-flex', color: 'var(--warning)' }}>
              <WifiOff size={16} />
            </span>
          )}
        </div>
        <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
          <span style={{ fontSize: '14px', fontWeight: 500, color: 'var(--text-muted)' }}>
            Olá, {user?.Nome || 'Usuário'}
          </span>
          <button 
            onClick={logout} 
            className="btn-text" 
            style={{ padding: '6px', borderRadius: 'var(--radius-sm)', display: 'inline-flex' }}
            title="Sair"
          >
            <LogOut size={18} />
          </button>
        </div>
      </header>

      {/* Conteúdo Principal */}
      <main style={{ flex: 1, paddingBottom: '84px', overflowY: 'auto' }}>
        {children}
      </main>

      {/* Navegação Inferior Premium */}
      <nav className="glass" style={{
        position: 'absolute',
        bottom: 0,
        left: 0,
        right: 0,
        height: '72px',
        display: 'flex',
        justifyContent: 'space-around',
        alignItems: 'center',
        borderTop: '1px solid var(--border-color)',
        zIndex: 100
      }}>
        <Link to="/" className={`nav-item ${activeTab === 'lists' ? 'active' : ''}`}>
          <ClipboardList size={22} />
          <span>Listas</span>
        </Link>
        <Link to="/environments" className={`nav-item ${activeTab === 'environments' ? 'active' : ''}`}>
          <MapPin size={22} />
          <span>Ambientes</span>
        </Link>
        <Link to="/markets" className={`nav-item ${activeTab === 'markets' ? 'active' : ''}`}>
          <Store size={22} />
          <span>Mercados</span>
        </Link>
        <Link to="/items" className={`nav-item ${activeTab === 'items' ? 'active' : ''}`}>
          <ShoppingBag size={22} />
          <span>Itens</span>
        </Link>
      </nav>

      {/* Estilos específicos da navegação */}
      <style>{`
        .nav-item {
          display: flex;
          flex-direction: column;
          align-items: center;
          gap: 4px;
          color: var(--text-muted);
          text-decoration: none;
          font-size: 12px;
          font-weight: 500;
          width: 25%;
          transition: all var(--transition-fast);
        }
        .nav-item:hover {
          color: var(--primary);
        }
        .nav-item.active {
          color: var(--primary);
        }
        .nav-item.active svg {
          transform: scale(1.1);
        }
        .nav-item svg {
          transition: transform var(--transition-fast);
        }
      `}</style>
    </div>
  );
}

function App() {
  const setOnline = useStore((state) => state.setOnline);

  // Monitora alterações na conexão de internet
  useEffect(() => {
    const handleOnline = () => {
      setOnline(true);
      runSync();
    };
    const handleOffline = () => setOnline(false);

    window.addEventListener('online', handleOnline);
    window.addEventListener('offline', handleOffline);

    // Executa também ao carregar se estiver online
    if (navigator.onLine) {
      runSync();
    }

    return () => {
      window.removeEventListener('online', handleOnline);
      window.removeEventListener('offline', handleOffline);
    };
  }, [setOnline]);

  return (
    <BrowserRouter>
      <Routes>
        {/* Rota Pública */}
        <Route path="/login" element={<Login />} />

        {/* Rotas Protegidas */}
        <Route path="/" element={<PrivateRoute><AppLayout><Dashboard /></AppLayout></PrivateRoute>} />
        <Route path="/environments" element={<PrivateRoute><AppLayout><Environments /></AppLayout></PrivateRoute>} />
        <Route path="/markets" element={<PrivateRoute><AppLayout><Markets /></AppLayout></PrivateRoute>} />
        <Route path="/items" element={<PrivateRoute><AppLayout><Items /></AppLayout></PrivateRoute>} />
        <Route path="/list/:id" element={<PrivateRoute><AppLayout><ListDetails /></AppLayout></PrivateRoute>} />
        <Route path="/merge" element={<PrivateRoute><AppLayout><MergeLists /></AppLayout></PrivateRoute>} />
        <Route path="/shopping/:id" element={<PrivateRoute><AppLayout><ShoppingMode /></AppLayout></PrivateRoute>} />

        {/* Redirecionamento Padrão */}
        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
