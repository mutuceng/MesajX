import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { User } from "../../constants/types/IUser";
import { FieldValues } from "react-hook-form";
import requests from "../../api/requests";
import { UserInfo } from "../../constants/types/IUserInfo";

interface AccountState {
  userProfiles: Record<string, UserInfo>;
  user: User | null;
}

const initialState: AccountState = {
  user: null,
  userProfiles: {},
};

export const registerUser = createAsyncThunk<any, FormData, { rejectValue: { error: any } }>(
  'user/registers',
  async (formData, { rejectWithValue }) => {
    try {
      const response = await requests.Account.register(formData);
      return response;
    } catch (error: any) {
      return rejectWithValue({ error: error.data });
    }
  }
);

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
  { userId: string }, // Dönüş türü
  string, // Parametre türü (username)
  { rejectValue: string }
>(
  "account/getUserIdByUsername",
  async (username, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getUserIdByUsername(username);
      return response; // { userId: string } dönmeli
    } catch (error: any) {
      return rejectWithValue(error.message || "Kullanıcı bulunamadı");
    }
  }
);

export const fetchUserProfile = createAsyncThunk<
  UserInfo,
  string,
  { rejectValue: string }
>(
  "account/fetchUserProfile",
  async (userId, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getUserProfile(userId);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Failed to fetch user profile");
    }
  }
);

export const fetchMultipleUserProfiles = createAsyncThunk<
  UserInfo[],
  string[],
  { rejectValue: string }
>(
  "account/fetchMultipleUserProfiles",
  async (userIds, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getMultipleUserProfiles(userIds);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Failed to fetch user profiles");
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
