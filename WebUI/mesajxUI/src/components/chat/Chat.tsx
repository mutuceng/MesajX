import React, { useEffect, useState } from "react";
import * as signalR from "@microsoft/signalr";
import ChatWindow from "./chatwindow/ChatWindow";
import MessageInput from "./messageinput/MessageInput";

interface Message {
  senderId: string;
  text: string;
  timestamp: string;
}

const Chat: React.FC = () => {
  const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
  const [messages, setMessages] = useState<Message[]>([]);
  const currentUser = "user1";

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
          connection.on("ReceiveMessage", (message: string, senderId: string) => {
            const timestamp = new Date().toLocaleTimeString([], {
              hour: "2-digit",
              minute: "2-digit",
            });
            setMessages((prev) => [...prev, { text: message, senderId, timestamp }]);
          });
        })
        .catch((err) => console.error("Bağlantı hatası: ", err));
    }
  }, [connection]);

  useEffect(() => {
    console.log("Gelen mesajlar:", messages);
  }, [messages]);

  const handleSend = async (text: string) => {
    if (connection) {
      try {
        const timestamp = new Date().toLocaleTimeString([], {
          hour: "2-digit",
          minute: "2-digit",
        });
        await connection.invoke("SendMessageAsync", text, currentUser);
        setMessages((prev) => [...prev, { text, senderId: currentUser, timestamp }]);
      } catch (err) {
        console.error("Mesaj gönderilemedi:", err);
      }
    }
  };

  return (
    <div className="flex flex-col flex-1 h-full">
      <ChatWindow messages={messages} currentUser={currentUser} />
      <MessageInput onSend={handleSend} />
    </div>
  );
};

export default Chat;