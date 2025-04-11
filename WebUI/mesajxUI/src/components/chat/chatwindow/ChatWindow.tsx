import React, { useRef, useEffect } from "react";
import "./ChatWindow.css";

interface Message {
  senderId: string;
  text: string;
  timestamp: string;
}

interface ChatWindowProps {
  messages: Message[];
  currentUser: string;
}

const ChatWindow: React.FC<ChatWindowProps> = ({ messages, currentUser }) => {
  const endRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    endRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  return (
    <div className="chat-window">
      <div className="message-container">

      <div className="chat chat-start">
  <div className="chat-image avatar">
    <div className="w-10 rounded-full">
      <img
        alt="Tailwind CSS chat bubble component"
        src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
    </div>
  </div>
  <div className="chat-bubble">It was said that you would, destroy the Sith, not join them.</div>
</div>
<div className="chat chat-start">
  <div className="chat-image avatar">
    <div className="w-10 rounded-full">
      <img
        alt="Tailwind CSS chat bubble component"
        src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
    </div>
  </div>
  <div className="chat-bubble">It was you who would bring balance to the Force</div>
</div>
<div className="chat chat-start">
  <div className="chat-image avatar">
    <div className="w-10 rounded-full">
      <img
        alt="Tailwind CSS chat bubble component"
        src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp" />
    </div>
  </div>
  <div className="chat-bubble">Not leave it in Darkness</div>
</div>
        {messages.map((msg, i) => (
          <div
            key={i}
            className={`message ${
              msg.senderId === currentUser ? "outgoing" : "incoming"
            }`}
          >
            <div
              className={`flex items-end ${
                msg.senderId === currentUser
                  ? "flex-row-reverse space-x-reverse"
                  : "flex-row space-x-2"
              }`}
            >
              {/* Kullanıcı Avatarı */}
              <div className="avatar">
                {msg.senderId.charAt(0).toUpperCase()}
              </div>
              <div className="message-content">
                <div className="bubble">{msg.text}</div>
                <div className="timestamp">{msg.timestamp}</div>
              </div>


            </div>
          </div>
        ))}
        <div ref={endRef} />
      </div>
    </div>
  );
};

export default ChatWindow;