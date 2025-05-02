import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { User } from "../../constants/types/IUser";
import { FieldValues } from "react-hook-form";
import requests from "../../api/requests";

interface AccountState {
  user: User | null;
}

const initialState: AccountState = {
  user: null,
};

// LOGIN THUNK
export const loginUser = createAsyncThunk<User, FieldValues>(
  "user/Auth/login",
  async (userData, { rejectWithValue }) => {
    try {
      const user = await requests.Account.login(userData); // Beklenen response: { username, token }
      localStorage.setItem("user", JSON.stringify(user));
      return user;
    } catch (error: any) {
      return rejectWithValue({ error: error.data });
    }
  }
);

export const getUserIdByUsername = createAsyncThunk<
  string,
  string,
  { rejectValue: string }
>(
  "account/getUserIdByUsername",
  async (username, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getUserIdByUsername(username);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Kullanıcı bulunamadı");
    }
  }
);

// ACCOUNT SLICE
export const accountSlice = createSlice({
  name: "account",
  initialState,
  reducers: {
    logOut: (state) => {
      state.user = null;
      localStorage.removeItem("user");
      // navigate kullanılamaz burada — component içinde çağırılmalı!
    },
    setUser: (state, action) => {
      state.user = action.payload;
    },
  },
  extraReducers: (builder) => {
    builder.addCase(loginUser.fulfilled, (state, action) => {
      state.user = action.payload;
    });
  },
});



export const { logOut, setUser } = accountSlice.actions;
export default accountSlice.reducer;
