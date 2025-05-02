import { configureStore } from '@reduxjs/toolkit';
import { accountSlice } from '../features/account/accountSlice';
import { chatRoomSlice } from '../features/chat/chatRoomSlice';
import  chatRoomMemberSlice from '../features/chat/chatRoomMemberSlice.ts';
import messageSlice from '../features/chat/messageSlice.ts';

export const store = configureStore({
  reducer: {
    account: accountSlice.reducer,
    chatRoom: chatRoomSlice.reducer,
    chatRoomMember: chatRoomMemberSlice,
    message: messageSlice,
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; 