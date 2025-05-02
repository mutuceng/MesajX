import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import requests from "../../api/requests"

interface Message {
  _id: string;
  senderId: string;
  text?: string;
  image?: string | null;
  createdAt: string;
}

interface MessageState {
  messages: Message[];
  status: "idle" | "loading" | "succeeded" | "failed";
  error: string | null;
  selectedChatRoomId: string | null;
}

const initialState: MessageState = {
  messages: [],
  status: "idle",
  error: null,
  selectedChatRoomId: null,
};

export const fetchMessages = createAsyncThunk(
    "message/fetchMessages",
    async (args: { chatRoomId: string; page?: number }, { rejectWithValue }) => {
      const { chatRoomId, page = 1 } = args; // page opsiyonel, varsayılan 1
      try {
        const response = await requests.Message.getMessagesByRoomId(chatRoomId, page);
        return response;
      } catch (error: any) {
        return rejectWithValue(error.response?.data?.error || "Mesajlar alınamadı");
      }
    }
  );

export const sendMessage = createAsyncThunk(
  "message/sendMessage",
  async (data: { chatRoomId: string; text: string }, { rejectWithValue }) => {
    try {
      const response = await requests.Message.sendMessage({ chatRoomId: data.chatRoomId, text: data.text });
      return response;
    } catch (error: any) {
      return rejectWithValue(error.response?.data?.error || "Mesaj gönderilemedi");
    }
  }
);

const messageSlice = createSlice({
  name: "message",
  initialState,
  reducers: {
    setSelectedChatRoomId: (state, action) => {
      state.selectedChatRoomId = action.payload;
      console.log("messageSlice - setSelectedChatRoomId çalıştı, yeni değer:", state.selectedChatRoomId);
    },
    clearMessages: (state) => {
      state.messages = [];
      state.selectedChatRoomId = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchMessages.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchMessages.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.messages = action.payload; // Backend'den dönen mesaj listesi
      })
      .addCase(fetchMessages.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload as string;
      })
      .addCase(sendMessage.fulfilled, (state, action) => {
        state.messages.push(action.payload); // Yeni mesajı ekle
      });
  },
});

export const { setSelectedChatRoomId, clearMessages } = messageSlice.actions;
export default messageSlice.reducer;