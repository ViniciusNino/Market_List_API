import { create } from 'zustand';

// Tenta recuperar a sessão anterior do LocalStorage
const savedUser = localStorage.getItem('ml_user') ? JSON.parse(localStorage.getItem('ml_user')) : null;
const savedToken = localStorage.getItem('ml_token') || null;

export const useStore = create((set) => ({
  user: savedUser,
  token: savedToken,
  online: navigator.onLine,
  syncing: false,

  login: (user, token) => {
    localStorage.setItem('ml_user', JSON.stringify(user));
    localStorage.setItem('ml_token', token);
    set({ user, token });
  },

  logout: () => {
    localStorage.removeItem('ml_user');
    localStorage.removeItem('ml_token');
    set({ user: null, token: null });
  },

  setOnline: (online) => set({ online }),
  setSyncing: (syncing) => set({ syncing }),
  
  updateUser: (fields) => set((state) => {
    const updatedUser = { ...state.user, ...fields };
    localStorage.setItem('ml_user', JSON.stringify(updatedUser));
    return { user: updatedUser };
  })
}));
