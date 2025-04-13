import axios from 'axios';
import { useAuthStore } from '../store/AuthStore'; // AuthStore'dan useAuthStore'u içe aktar

export const axiosInstance = axios.create({
  baseURL: 'https://api.myapp.com', // API Gateway'in adresi
  headers: {
    'Content-Type': 'application/json',
  },
});

// İstek interceptor'ı: Her istekte token ekle
axiosInstance.interceptors.request.use((config) => {
//   const token = useAuthStore.getState().token; // useAuthStore'dan token al
//   if (token) {
//     config.headers.Authorization = `Bearer ${token}`;
//   }
  return config;
});

// Yanıt interceptor'ı: Hata yönetimi
axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    // C# backend'inden gelen hata mesajlarını işle
    const message = error.response?.data?.message || error.message || 'An error occurred';
    return Promise.reject(new Error(message));
  }
);