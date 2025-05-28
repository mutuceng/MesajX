import React, { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { startSignalRConnection } from "../api/SignalRService";
import { fetchMessages } from "../features/chat/messageSlice";
import ChatHeader from "./ChatHeader";
import ChatWindow from "./ChatWindow";
import MessageInput from "./MessageInput";

const Chat: React.FC = () => {
  const token = useAppSelector((state) => state.account.user?.accessToken);
  const dispatch = useAppDispatch();
  const selectedChatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);

  // Mesajları yükle
  useEffect(() => {
    if (selectedChatRoomId) {
      dispatch(fetchMessages({ chatRoomId: selectedChatRoomId }));
    }
  }, [selectedChatRoomId, dispatch]);

  // SignalR bağlantısını başlat
  useEffect(() => {
    if (token && selectedChatRoomId) {
      startSignalRConnection(token, selectedChatRoomId);
    }

    return () => {
      // Bağlantıyı durdurma işlemi
      // stopSignalRConnection(); (isteğe bağlı)
    };
  }, [token, selectedChatRoomId]);

  return (
    <div className="flex flex-col h-full">
      <ChatHeader />

      <div className="flex-1 overflow-y-auto">
        <ChatWindow />
      </div>

      <MessageInput />
    </div>
  );
};

export default Chat;
