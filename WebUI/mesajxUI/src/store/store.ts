import { configureStore } from '@reduxjs/toolkit';
import { accountSlice } from '../features/account/accountSlice';
import { userInfoSlice } from '../features/userInfo/userInfoSlice';

export const store = configureStore({
  reducer: {
    account: accountSlice.reducer,
    userInfo: userInfoSlice.reducer,
  }
});

export type RootState = ReturnType<typeof store.getState>; 
export type AppDispatch = typeof store.dispatch; 