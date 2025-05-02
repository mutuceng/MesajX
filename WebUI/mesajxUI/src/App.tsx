import Navbar from "./components/Navbar";

import HomePage from "./pages/HomePage";
import SignUpPage from "./pages/SignUpPage";
import SignInPage from "./pages/SignInPage";
import SettingsPage from "./pages/SettingsPage";

import { Routes, Route, Navigate, useNavigate } from "react-router-dom";
import { Toaster } from "react-hot-toast";
import { useThemeStore } from "./store/ThemeStore";
import { useAppDispatch, useAppSelector } from "./hooks/hooks";
import { useCallback, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import { logOut, setUser } from "./features/account/accountSlice";
import requests from "./api/requests";
import { User } from "./constants/types/IUser";
import React from "react";

const App = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const { user } = useAppSelector((state) => state.account);
  const { theme } = useThemeStore();
  const [loading, setLoading] = React.useState(true); // Yükleme durumu

  const isTokenExpired = (token: string): boolean => {
    try {
      const decoded: any = jwtDecode(token);
      const currentTime = Date.now() / 1000;
      return decoded.exp < currentTime;
    } catch (error) {
      return true;
    }
  };

  const checkTokenValidity = useCallback(
    async (storedUser: User) => {
      if (isTokenExpired(storedUser.accessToken)) {
        try {
          console.log("🔁 Refresh token yenileniyor...");
          const newUser = await requests.Account.refreshToken(storedUser.refreshToken);
          dispatch(setUser(newUser));
          localStorage.setItem("user", JSON.stringify(newUser));
        } catch (error) {
          console.error("🔒 Token yenileme başarısız:", error);
          dispatch(logOut());
          navigate("/login");
        }
      }
    },
    [dispatch, navigate]
  );

  useEffect(() => {
    const initializeUser = async () => {
      console.log("App.tsx useEffect çalıştı");
      const storedUser = JSON.parse(localStorage.getItem("user") || "null") as User | null;

      if (storedUser) {
        dispatch(setUser(storedUser));
        await checkTokenValidity(storedUser);
      }
      setLoading(false); // Yükleme tamamlandı
    };

    initializeUser();
  }, [dispatch, checkTokenValidity]);

  console.log("Current User:", user);

  // Yükleme sırasında bir şey göstermemek için
  if (loading) {
    return <div>Loading...</div>;
  }

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