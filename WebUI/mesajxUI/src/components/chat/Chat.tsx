import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import ChatWindow from "./chatwindow/ChatWindow";
import { v4 as uuidv4 } from "uuid"; // benzersiz _id üretmek için

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
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
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

  useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:5001/chathub", { withCredentials: true })
      .withAutomaticReconnect()
      .build();

    setConnection(newConnection);
  }, []);

  useEffect(() => {
    if (connection) {
      connection
        .start()
        .then(() => {
          console.log("SignalR bağlantısı kuruldu!");

          connection.on("ReceiveMessage", (text: string, senderId: string) => {
            const newMsg: Message = {
              _id: uuidv4(),
              senderId,
              text,
              createdAt: new Date().toISOString(),
              image: null,
            };
            setMessages((prev) => [...prev, newMsg]);
          });
        })
        .catch((err) => console.error("Bağlantı hatası: ", err));
    }
  }, [connection]);

  const handleSend = async (text: string) => {
    if (connection) {
      try {
        await connection.invoke("SendMessageAsync", text, authUser._id);
        const newMsg: Message = {
          _id: uuidv4(),
          senderId: authUser._id,
          text,
          createdAt: new Date().toISOString(),
          image: null,
        };
        setMessages((prev) => [...prev, newMsg]);
      } catch (err) {
        console.error("Mesaj gönderilemedi:", err);
      }
    }
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
