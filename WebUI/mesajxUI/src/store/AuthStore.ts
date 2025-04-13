import { create } from 'zustand';
import type { AuthUser } from '../types/AuthUser';
import type { AuthState } from '../types/AuthState';

export const useAuthStore = create<AuthState>((set) => ({
  authUser: null,
  isAuthenticated: false,
  isUpdatingProfile: false,

  // Kullanıcıyı oturum açma
  login: (user: AuthUser) => set({ authUser: user, isAuthenticated: true }),

  // Kullanıcıyı oturum kapama
  logout: () => set({ authUser: null, isAuthenticated: false }),

  // Kullanıcı bilgilerini güncelleme
  updateProfile: (profileData: Partial<AuthUser>) =>
    set((state) => {
      if (state.authUser) {
        const updatedUser = { ...state.authUser, ...profileData };
        return { authUser: updatedUser };
      }
      return state;
    }),

  // Profil güncelleme durumunu set etme
  setUpdatingProfile: (isUpdating: boolean) => set({ isUpdatingProfile: isUpdating }),
}));
