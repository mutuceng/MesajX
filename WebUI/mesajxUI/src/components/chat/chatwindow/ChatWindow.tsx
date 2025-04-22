import React, { useEffect, useRef, useState } from "react";
import ChatHeader from "../../ChatHeader";
import MessageInput from "../messageinput/MessageInput";


interface User {
  _id: string;
  profilePic?: string;
}

interface Message {
  _id: string;
  senderId: string;
  text?: string;
  image?: string | null;
  createdAt: string;
}

interface ChatWindowProps {
  authUser: User;
  selectedUser: User;
  messages: Message[];
  onSend: (text: string) => void;
}

const ChatWindow: React.FC<ChatWindowProps> = ({ authUser, selectedUser }) => {
  const [messages, setMessages] = useState<Message[]>([]);
  const messageEndRef = useRef<HTMLDivElement | null>(null);

  useEffect(() => {
    const dummyMessages: Message[] = [
      {
        _id: "1",
        senderId: authUser._id,
        text: "Selam! Nasılsın?",
        image: null,
        createdAt: new Date().toISOString(),
      },
      {
        _id: "2",
        senderId: selectedUser._id,
        text: "İyiyim sen nasılsın?",
        image: null,
        createdAt: new Date().toISOString(),
      },
      {
        _id: "3",
        senderId: selectedUser._id,
        image: "https://placekitten.com/200/300",
        createdAt: new Date().toISOString(),
      },
      {
        _id: "4",
        senderId: authUser._id,
        text: "Bu da fotoğraf 🐱",
        image: "https://placekitten.com/250/300",
        createdAt: new Date().toISOString(),
      },
    ];

    setMessages(dummyMessages);
  }, [authUser._id, selectedUser._id]);

  function onSend(message: string): void {
    throw new Error("Function not implemented.");
  }

  return (
    <div className="flex-1 flex flex-col overflow-auto">
      <ChatHeader />

      <div className="flex-1 overflow-y-auto p-4 space-y-4">
        {messages.map((message) => (
          <div
            key={message._id}
            className={`chat ${
              message.senderId === authUser._id ? "chat-end" : "chat-start"
            }`}
            ref={messageEndRef}
          >
            <div className="chat-image avatar">
              <div className="size-10 rounded-full border">
                <img
                  src={
                    message.senderId === authUser._id
                      ? authUser.profilePic || "/avatar.png"
                      : selectedUser.profilePic || "/avatar.png"
                  }
                  alt="profile pic"
                />
              </div>
            </div>
            <div className="chat-header mb-1">
              <time className="text-xs opacity-50 ml-1">
                {new Date(message.createdAt).toLocaleTimeString()}
              </time>
            </div>
            <div className="chat-bubble flex flex-col">
              {message.image && (
                <img
                  src={message.image}
                  alt="Attachment"
                  className="sm:max-w-[200px] rounded-md mb-2"
                />
              )}
              {message.text && <p>{message.text}</p>}
            </div>
          </div>
        ))}
      </div>

      <MessageInput onSend={onSend} />

    </div>
  );
};

export default ChatWindow;
