import React, { useState } from "react";
import ChatWindow from "./chatwindow/ChatWindow";

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

const Chat: React.FC = () => {
  const [messages, setMessages] = useState<Message[]>([]);

  // Sadece örnek kullanıcılar
  const authUser: User = {
    _id: "user1",
    profilePic: "https://placekitten.com/100/100",
  };

  const selectedUser: User = {
    _id: "user2",
    profilePic: "https://placekitten.com/101/101",
  };
  
  const handleSend = async (text: string) => {
      console.log("Sending message:", text);
  };

  return (
    <div className="flex flex-col flex-1 h-full">
      <ChatWindow
        authUser={authUser}
        selectedUser={selectedUser}
        messages={messages}
        onSend={handleSend}
      />
    </div>
  );
};

export default Chat;
