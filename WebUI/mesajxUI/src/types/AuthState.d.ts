export interface AuthState {
    authUser: AuthUser | null;
    isAuthenticated: boolean;
    isUpdatingProfile: boolean;
    login: (user: AuthUser) => void;
    logout: () => void;
    updateProfile: (profileData: Partial<AuthUser>) => void;
    setUpdatingProfile: (isUpdating: boolean) => void;
  }