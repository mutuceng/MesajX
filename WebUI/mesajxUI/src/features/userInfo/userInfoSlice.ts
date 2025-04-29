// src/store/userInfoSlice.ts
import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { UserInfo } from "../../constants/types/IUserInfo";

interface UserInfoState {
  userInfo: UserInfo | null;
}

const initialState: UserInfoState = {
  userInfo: null,
};

export const userInfoSlice = createSlice({
  name: "userInfo",
  initialState,
  reducers: {
    setUserInfo: (state, action: PayloadAction<UserInfo | null>) => {
      state.userInfo = action.payload;
    },
    clearUserInfo: (state) => {
      state.userInfo = null;
    },
  },
});

export const { setUserInfo, clearUserInfo } = userInfoSlice.actions;
