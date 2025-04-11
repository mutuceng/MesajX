// import React, { useEffect, useState } from "react";
// import * as signalR from "@microsoft/signalr";

// const Chat: React.FC = () => {
//   const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
//   const [messages, setMessages] = useState<string[]>([]);
//   const [message, setMessage] = useState("");
 

//   useEffect(() => {
//     const newConnection = new signalR.HubConnectionBuilder()
//       .withUrl("http://localhost:5001/chathub", {
//         withCredentials: true 
//       })
//       .withAutomaticReconnect()
//       .build();

//     setConnection(newConnection);
//   }, []);

//   useEffect(() => {
//     if (connection) {
//       connection
//         .start()
//         .then(() => {
//           console.log("SignalR bağlantısı kuruldu!");

//           connection.on("ReceiveMessage", (message: string) => {
//             setMessages((prevMessages) => [...prevMessages, message]);
//           });
//         })
//         .catch((error) => console.log("Bağlantı hatası: ", error));
//     }
//   }, [connection]);

//   const sendMessage = async () => {
//     if (connection && message.trim()) {
//       await connection.invoke("SendMessageAsync", message);
//       setMessage("");
//     }
//   };


//   return (
//     <div>
//       <h2>Chat Uygulaması</h2>
//       <div>
//         {messages.map((msg, index) => (
//           <div key={index}>{msg}</div>
//         ))}
//       </div>
//       <input
//         type="text"
//         value={message}
//         onChange={(e) => setMessage(e.target.value)}
//       />
//       <button onClick={sendMessage}>Gönder</button>
//     </div>
//   );
// };

// export default Chat;
