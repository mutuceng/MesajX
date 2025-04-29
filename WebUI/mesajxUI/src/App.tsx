import Navbar from "./components/Navbar";

import HomePage from "./pages/HomePage";
import SignUpPage from "./pages/SignUpPage";
import SignInPage from "./pages/SignInPage";
import SettingsPage from "./pages/SettingsPage";

import { Routes, Route, Navigate } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import { useThemeStore } from "./store/ThemeStore";
import { useAppDispatch, useAppSelector } from "./hooks/hooks";
import { useEffect } from "react";
import { setUser } from "./features/account/accountSlice";
import requests from "./api/requests";

const App = () => {
  const dispatch = useAppDispatch();
  const { user } = useAppSelector(state => state.account);
  const { theme } = useThemeStore();

  useEffect(() => {
    console.log("App.tsx useEffect çalıştı");
    const storedUser = JSON.parse(localStorage.getItem("user") || "null");

    if (storedUser) {
      dispatch(setUser(storedUser));
      checkTokenValidity(storedUser);
    }
  }, []);

  const checkTokenValidity = async (storedUser: any) => {
    const currentTime = Date.now() / 1000;

    if (storedUser.expiresIn < currentTime) {
      try {
        console.log("Refresh token yenileniyor...");

        const newUser = await requests.Account.refreshToken(storedUser.refreshToken);
        dispatch(setUser(newUser));
        localStorage.setItem("user", JSON.stringify(newUser));
      } catch (error) {
        console.error("🔒 Token yenileme başarısız:", error);
        // İsteğe bağlı: logout işlemi yapılabilir
      }
    }
  };

  return (
    <div data-theme={theme}>
      <Navbar />

      <Routes>
        <Route path="/" element={user ? <HomePage /> : <Navigate to="/login" replace />} />
        <Route path="/login" element={user ? <Navigate to="/" replace /> : <SignInPage />} />
        <Route path="/signup" element={user ? <Navigate to="/" replace /> : <SignUpPage />} />
        <Route path="/settings" element={user ? <SettingsPage /> : <Navigate to="/login" replace />} />
      </Routes>

      <Toaster />
    </div>
  );
};

export default App;
