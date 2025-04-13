import { create } from "zustand";

interface ThemeState {
  theme: string;
  setTheme: (theme: string) => void;
}

export const useThemeStore = create<ThemeState>((set) => ({
  theme: typeof window !== "undefined" 
    ? localStorage.getItem("chat-theme") || "sunset" 
    : "sunset",
  setTheme: (theme: string) => {
    if (typeof window !== "undefined") {
      localStorage.setItem("chat-theme", theme);
      document.documentElement.setAttribute("data-theme", theme); // Tema değişikliğini uygula
    }
    set({ theme });
  },
}));