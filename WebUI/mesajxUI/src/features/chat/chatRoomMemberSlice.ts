import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import requests from "../../api/requests";

enum Role {
    Moderator = 0,
    Member = 1,
}

interface Member {
    UserId: string;
    ChatRoomId: string;
    Role: Role;
}

export const addMember = createAsyncThunk<
  Member,
  { chatRoomId: string; userId: string; role?: Role },
  { rejectValue: string }
>(
  "member/addMember",
  async ({ chatRoomId, userId, role = Role.Member }, { rejectWithValue }) => {
    try {
      const memberData = {
        UserId: userId,
        ChatRoomId: chatRoomId,
        Role: role,
      };
      const response = await requests.ChatRoomMember.addMember(memberData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Üye eklenemedi");
    }
  }
);

export const removeMember = createAsyncThunk<
  { userId: string; chatRoomId: string },
  { userId: string; chatRoomId: string },
  { rejectValue: string }
>(
  "member/removeMember",
  async ({ userId, chatRoomId }, { rejectWithValue }) => {
    try {
      await requests.ChatRoomMember.removeMember(userId, chatRoomId);
      return { userId, chatRoomId };
    } catch (error: any) {
      return rejectWithValue(error.message || "Üye silinemedi");
    }
  }
);

const chatRoomMemberSlice = createSlice({
    name: "chatRoomMember",
    initialState: {
        loading: false,
        error: null as string | null,
        success: false,
    },
    reducers: {
        resetMemberState: (state) => {
            state.loading = false;
            state.error = null;
            state.success = false;
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(addMember.pending, (state) => {
                state.loading = true;
                state.error = null;
                state.success = false;
            })
            .addCase(addMember.fulfilled, (state, action) => {
                state.loading = false;
                state.success = true;
            })
            .addCase(addMember.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload as string;
                state.success = false;
            });
    },
});

export const { resetMemberState } = chatRoomMemberSlice.actions;
export default chatRoomMemberSlice.reducer;
