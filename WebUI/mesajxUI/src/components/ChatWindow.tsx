import React, { useEffect, useRef } from "react";
import { useAppSelector } from "../hooks/hooks";

const ChatWindow = () => {
  const messages = useAppSelector((state) => state.message.messages);
  const endOfMessagesRef = useRef<HTMLDivElement>(null);
  const currentUserId = useAppSelector((state) => state.account?.user?.userId); // Kullanıcının ID'sini al

  // Mesajlar değiştiğinde en aşağıya kaydır
  useEffect(() => {
    endOfMessagesRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const validMessages = messages.filter(msg => typeof msg.content === 'string' && msg.content.trim() !== '');

  return (
    <div className="p-4 overflow-y-auto h-full">
      {/* Her bir mesajı ayrı bir kartla göster */}
      {validMessages.map((msg, index) => {
        const isCurrentUser = msg.userId === currentUserId;

        return (
          <div
            key={msg.messageId || index}
            className={`mb-3 p-2 max-w-3/4 ${isCurrentUser
              ? "ml-auto text-right rounded-l-lg rounded-t-lg text-white"
              : "mr-auto rounded-r-lg rounded-t-lg"
            }`}
            style={{
              width: "fit-content",
              maxWidth: "75%",
              backgroundColor: isCurrentUser
                ? "oklch(0.65 0.15 145 / 1)"
                : "oklch(0.58 0.23 275.12 / 1)"
            }}
          >
            <div className="font-bold">{msg.userId}</div>
            <div className="mt-1">{msg.content}</div>
          </div>
        )
      })}
      <div ref={endOfMessagesRef} />
    </div>
  );
};

export default ChatWindow;