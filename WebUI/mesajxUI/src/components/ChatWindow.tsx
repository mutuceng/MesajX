import React, { useEffect, useRef } from "react";
import { useAppSelector, useAppDispatch } from "../hooks/hooks";
import { fetchChatUsers } from "../features/account/accountSlice";

const API_BASE_URL = "http://localhost:5281";

const ChatWindow = () => {
  const dispatch = useAppDispatch();
  const messages = useAppSelector((state) => state.message.messages);
  const selectedChatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);
  const messageStatus = useAppSelector((state) => state.message.status);
  const chatUsers = useAppSelector((state) => state.account.chatUsers);
  const currentUserId = useAppSelector((state) => state.account?.user?.userId);
  const endOfMessagesRef = useRef<HTMLDivElement>(null);

  // Ensure messages is always an array
  const safeMessages = Array.isArray(messages) ? messages : [];

  // Mesajlar değiştiğinde en aşağıya kaydır
  useEffect(() => {
    endOfMessagesRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [safeMessages]);

  // Chat kullanıcılarını getir
  useEffect(() => {
    if (safeMessages.length > 0 && selectedChatRoomId) {
      const userIds = [...new Set(
        safeMessages
          .filter(msg => msg.userId !== currentUserId)
          .map(msg => msg.userId)
          .filter(userId => userId) 
      )];
      
      console.log("Chat userIds to fetch:", userIds);
      console.log("Current chatUsers in store:", chatUsers);
      
      if (userIds.length > 0) {
        // Halihazırda yüklenmemiş kullanıcıları filtrele
        const unloadedUserIds = userIds.filter(userId => 
          !chatUsers.some(user => user.id === userId)
        );
        
        if (unloadedUserIds.length > 0) {
          dispatch(fetchChatUsers(unloadedUserIds))
            .then((result: any) => {
              console.log("fetchChatUsers result:", result);
            })
            .catch((error: any) => {
              console.error("fetchChatUsers error:", error);
            });
        }
      }
    }
  }, [safeMessages, currentUserId, dispatch, selectedChatRoomId, chatUsers]);

  // Kullanıcı bilgilerini ID'ye göre bul
  const getUserInfo = (userId: string) => {
    return Array.isArray(chatUsers) ? chatUsers.find(user => user.id === userId) : undefined;
  };

  const areUserInfosLoaded = (messageList: any[]) => {
    if (!currentUserId) return false;
    
    const otherUserIds = [...new Set(
      messageList
        .filter(msg => msg.userId !== currentUserId && msg.userId)
        .map(msg => msg.userId)
    )];
    
    return otherUserIds.length === 0 || otherUserIds.every(userId => 
      Array.isArray(chatUsers) && chatUsers.some(user => user.id === userId)
    );
  };

  if (!selectedChatRoomId) {
    return (
      <div className="p-4 text-center text-gray-500">
        Lütfen bir sohbet odası seçin.
      </div>
    );
  }

  if (messageStatus === "loading") {
    return (
      <div className="p-4 text-center text-gray-500">
        Mesajlar yükleniyor...
      </div>
    );
  }

  return (
    <div className="p-4 overflow-y-auto h-full">
      {safeMessages.length === 0 ? (
        <div className="text-center text-gray-500">
          Bu odada henüz mesaj yok.
        </div>
      ) : (
        safeMessages.map((msg, index) => {
          if (!msg || !msg.userId) {
            console.warn("Invalid message detected:", msg);
            return null;
          }
          const isCurrentUser = msg.userId === currentUserId;
          const userInfo = !isCurrentUser ? getUserInfo(msg.userId) : null;

          if (!isCurrentUser && !userInfo) {
            return null;
          }

          return (
            <div
              key={msg.messageId || index}
              className={`mb-3 flex ${isCurrentUser ? 'justify-end' : 'justify-start'}`}
            >
              {!isCurrentUser && (
                <div className="flex-shrink-0 mr-3">
                  <img
                    src={`${API_BASE_URL}${userInfo?.profileImageUrl}`}
                    alt={userInfo?.username || 'User'}
                    className="w-10 h-10 rounded-full object-cover"
                  />
                </div>
              )}
              <div
                className={`p-3 max-w-xs lg:max-w-md ${
                  isCurrentUser
                    ? "rounded-l-lg rounded-t-lg text-white"
                    : "rounded-r-lg rounded-t-lg text-white"
                }`}
                style={{
                  backgroundColor: isCurrentUser
                    ? "oklch(0.65 0.15 145 / 1)"
                    : "oklch(0.58 0.23 275.12 / 1)"
                }}
              >
                {!isCurrentUser && userInfo && (
                  <div className="font-bold text-sm mb-1 opacity-90">
                    {userInfo.username || 'Bilinmeyen Kullanıcı'}
                  </div>
                )}
                {msg.content && <div>{msg.content}</div>}
                {msg.image && (
                  <img src={`${API_BASE_URL}${msg.image}`} alt="Mesaj görseli" className="max-w-full rounded" />
                )}
              </div>
            </div>
          );
        }).filter(Boolean)
      )}
      <div ref={endOfMessagesRef} />
    </div>
  );
};

export default ChatWindow;