import { createSlice, createAsyncThunk, PayloadAction } from "@reduxjs/toolkit";
import requests from "../../api/requests";
import { v4 as uuidv4 } from "uuid";

interface Message {
  messageId: string;
  userId: string;
  content?: string;
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

export const sendMessage = createAsyncThunk<
  Message,
  Message,
  { rejectValue: string }
>(
  "message/sendMessage",
  async (message, { rejectWithValue }) => {
    try {
      const response = await requests.Message.sendMessage(message);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || "Mesaj gönderilemedi");
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
    addMessage: (state, action: PayloadAction<Message>) => {
      // Aynı messageId'ye sahip bir mesaj varsa, tekrar ekleme
      const messageExists = state.messages.some(
        (msg) => msg.messageId === action.payload.messageId
      );
      if (!messageExists) {
        state.messages.push(action.payload); // Yeni mesajı ekle
      }
    },
    setMessages: (state, action: { payload: Message[] }) => {
      state.messages = action.payload;
    },
    resetMessages: (state) => {
      state.messages = [];
      state.selectedChatRoomId = null;
      state.error = null;
    },
  },
  extraReducers: (builder) => {
    builder
      .addCase(fetchMessages.pending, (state) => {
        state.status = "loading";
      })
      .addCase(fetchMessages.fulfilled, (state, action) => {
        state.status = "succeeded";
        state.messages = action.payload; // Mesajları doğrudan set et
      })
      .addCase(fetchMessages.rejected, (state, action) => {
        state.status = "failed";
        state.error = action.payload as string;
      })
      .addCase(sendMessage.fulfilled, (state, action) => {
        const messageExists = state.messages.some(
          (msg) => msg.messageId === action.payload.messageId
        );
        if (!messageExists) {
          state.messages.push(action.payload);
        }
      });
  },
});

export const { setSelectedChatRoomId, addMessage, resetMessages, setMessages } = messageSlice.actions;
export default messageSlice.reducer;
