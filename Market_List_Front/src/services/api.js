import { useStore } from '../store/useStore';

const BASE_URL = 'https://localhost:5001';

async function request(endpoint, options = {}) {
  const token = useStore.getState().token;
  
  const headers = {
    'Content-Type': 'application/json',
    ...options.headers,
  };

  if (token) {
    headers['Authorization'] = `Bearer ${token}`;
  }

  const config = {
    ...options,
    headers,
  };

  try {
    const response = await fetch(`${BASE_URL}${endpoint}`, config);
    
    // Tratamento de token expirado ou não autorizado
    if (response.status === 401) {
      useStore.getState().logout();
      throw new Error('Sessão expirada. Por favor, faça login novamente.');
    }

    const data = await response.json();

    if (!response.ok) {
      throw new Error(data.message || data.response || 'Ocorreu um erro no servidor.');
    }

    return data.response !== undefined ? data.response : data;
  } catch (error) {
    console.error(`Erro na requisição ${endpoint}:`, error);
    throw error;
  }
}

export const api = {
  get: (endpoint, options) => request(endpoint, { ...options, method: 'GET' }),
  post: (endpoint, body, options) => request(endpoint, { ...options, method: 'POST', body: JSON.stringify(body) }),
  put: (endpoint, body, options) => request(endpoint, { ...options, method: 'PUT', body: JSON.stringify(body) }),
  delete: (endpoint, options) => request(endpoint, { ...options, method: 'DELETE' }),
};
