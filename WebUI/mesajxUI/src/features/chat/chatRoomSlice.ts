import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import requests from "../../api/requests";
import { v4 as uuidv4 } from "uuid";
import { RootState } from "../../store/store";

interface ChatRoom {
  [x: string]: any;
  chatRoomId: string;
  name: string;
  photoPath?: string;
  isGroup: boolean;
  createdAt: string;
}
interface ChatRoomState {
  [x: string]: any;
  rooms: ChatRoom[];
  activeRoom: any | null;
  status: "idle" | "loading" | "succeeded" | "failed";
  error: string | null;
  createdPhotoPath: string | null;
}

interface ChatRoomResponse {
  chatRoomId: string; // Backend'deki "ChatRoomId" ile uyumlu
  message?: string; // Backend'den dönen message alanı
  photoPath?: string;
}

export interface GroupData {
  name: string;
  photo?: File;
}

const initialState: ChatRoomState = {
  rooms: [],
  activeRoom: null,
  status: "idle",
  error: null,
  createdPhotoPath: null,
};

export const fetchChatRooms = createAsyncThunk<
ChatRoom[], // Dönen veri tipi
string, // userId tipi
{ rejectValue: string }>
(
  "chatRoom/fetchAll",
  async (userId: string) => {
    return await requests.ChatRoom.getRoomsByUserId(userId);
  }
);

export const createChatRoom = createAsyncThunk<
  ChatRoomResponse,
  GroupData,
  { rejectValue: string; state: RootState }
>(
  'chatRoom/createChatRoom',
  async (groupData: GroupData, { getState, rejectWithValue }) => {
    try {
      const state = getState();
      const userId = state.account.user?.userId;
      const token = state.account.user?.accessToken;

      if (!userId || !token) {
        throw new Error('Kullanıcı bilgileri veya token bulunamadı');
      }

      const formData = new FormData();
      formData.append('ChatRoomId', uuidv4());
      formData.append('Name', groupData.name);
      formData.append('IsGroup', 'true');
      formData.append('CreatedAt', new Date().toISOString());
      if (groupData.photo) {
        formData.append('groupImage', groupData.photo);
      }

      const response = await requests.ChatRoom.createChatRoom(formData);
      return response;
    } catch (error: any) {
      return rejectWithValue(error.message || 'Grup oluşturma hatası');
    }
  }
);

export const chatRoomSlice = createSlice({
  name: "chatRoom",
  initialState,
  reducers: {
    setSelectedRoom: (state, action) => {
      state.selectedRoom = action.payload;
      console.log("setSelectedRoom çalıştı, yeni değer:", state.selectedRoom); // Log ekle
    },
    clearCreatedPhotoPath: (state) => {
      state.createdPhotoPath = null;
    },
  },
  extraReducers: builder => {
    builder.addCase(fetchChatRooms.pending, state => {
      state.status = "loading";
    });
    builder.addCase(fetchChatRooms.fulfilled, (state, action) => {
      state.status = "succeeded";
      state.rooms = action.payload;
    });
    builder.addCase(fetchChatRooms.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.error.message || "ChatRoom fetch error";
    });

    builder.addCase(createChatRoom.pending, state => {
      state.status = "loading";
      state.error = null;
    });
    builder.addCase(createChatRoom.fulfilled, (state, action) => {
      state.status = "succeeded";
      state.rooms = [
        ...state.rooms,
        {
          chatRoomId: action.payload.chatRoomId,
          name: action.payload.message || "Unnamed Room",
          photoPath: action.payload.photoPath,
          isGroup: true,
          createdAt: new Date().toISOString(),
        },
      ];
      state.activeRoom = action.payload;
      state.createdPhotoPath = action.payload.photoPath || null;
    });
    builder.addCase(createChatRoom.rejected, (state, action) => {
      state.status = "failed";
      state.error = action.payload || "Grup oluşturma hatası";
    });
  },
});

export const { setSelectedRoom, clearCreatedPhotoPath } = chatRoomSlice.actions;

export default chatRoomSlice.reducer;