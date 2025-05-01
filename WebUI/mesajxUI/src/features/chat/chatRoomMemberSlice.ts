import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import requests from "../../api/requests";

export const addMember = createAsyncThunk(
    "chatRoomMember/addMember",
    async (memberData: { chatRoomId: string; userId: string ; role:number}, { rejectWithValue }) => {
        try {
            const response = await requests.ChatRoomMember.addMember(memberData);
            return response.data; 
        } catch (error: any) {
            return rejectWithValue(error.response?.data?.error || "Üye ekleme hatası");
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
