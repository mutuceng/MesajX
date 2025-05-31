import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { User } from "../../constants/types/IUser";
import { FieldValues } from "react-hook-form";
import requests from "../../api/requests";
import { UserInfo } from "../../constants/types/IUserInfo";

interface AccountState {
  userProfile: UserInfo;
  user: User | null;
  chatUsers: UserInfo[];
}

const initialState: AccountState = {
  user: null,
  userProfile: {} as UserInfo,
  chatUsers: [] as UserInfo[],
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
  void,
  { rejectValue: string }
>(
  "account/fetchUserProfile",
  async (_, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getUserProfile();
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Failed to fetch user profile");
    }
  }
);


export const fetchChatUsers = createAsyncThunk<
  UserInfo[],
  string[],
  { rejectValue: string }
>(
  "account/fetchChatUsers",
  async (userIds, { rejectWithValue }) => {
    try {
      const response = await requests.Account.getChatUsers(userIds);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Failed to fetch users");
    }
  }
);


export const updateUserProfile = createAsyncThunk(
  "account/updateUserProfile",
  async ({ userId, updateDto }: { userId: string; updateDto: {} }, thunkAPI) => {
    try {
      const response = await requests.Account.updateUserProfile(userId, updateDto);
      return response;
    } catch (error) {
      return thunkAPI.rejectWithValue("Profil güncellenemedi.");
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
    builder
      .addCase(loginUser.fulfilled, (state, action) => {
        state.user = action.payload;
      })
      .addCase(fetchChatUsers.fulfilled, (state, action) => {
        state.chatUsers = action.payload;
      })
      .addCase(fetchChatUsers.rejected, (state, action) => {
        console.error("Failed to fetch chat users:", action.payload);
      })
      .addCase(updateUserProfile.fulfilled, (state, action) => {
        state.userProfile = action.payload;
      });
      
    builder.addCase(fetchUserProfile.fulfilled, (state, action) => {
        state.userProfile = action.payload;
      });
  },
  
});



export const { logOut, setUser } = accountSlice.actions;
export default accountSlice.reducer;
