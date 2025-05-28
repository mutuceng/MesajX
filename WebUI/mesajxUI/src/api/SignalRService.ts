import * as signalR from "@microsoft/signalr";
import { store } from "../store/store";
import { addMessage } from "../features/chat/messageSlice";

let connection: signalR.HubConnection | null = null;

// Gönderilen mesajları takip etmek için bir Set
const sentMessageIds = new Set<string>();

export const startSignalRConnection = async (token: string, chatRoomId: string) => {
  connection = new signalR.HubConnectionBuilder()
    .withUrl("http://localhost:5281/chatHub", {
      accessTokenFactory: () => token,
    })
    .withAutomaticReconnect()
    .build();

  // Mesaj alma işleyicisini sadece bir kez eklediğimizden emin olmak için
  connection.off("ReceiveMessage"); // Önceki event listener'ları temizle
  connection.on("ReceiveMessage", (message) => {
    console.log("SignalR'dan gelen mesaj:", message);
    
    // Eğer bu mesajı biz göndermişsek (yani zaten işlenmişse), tekrar işleme
    if (sentMessageIds.has(message.messageId)) {
      console.log("Bu mesaj zaten gönderilmiş, tekrar işlenmiyor:", message.messageId);
      // Mesaj ID'sini Set'ten kaldırabilirsiniz (opsiyonel)
      sentMessageIds.delete(message.messageId);
      return;
    }

    // Mesaj zaten store'da var mı kontrol et
    const existingMessages = store.getState().message.messages;
    if (!existingMessages.find((msg) => msg.messageId === message.messageId)) {
      store.dispatch(addMessage(message));
    }
  });

  connection.onclose((error) => {
    console.warn("❌ SignalR bağlantısı kapandı:", error);
  });

  try {
    await connection.start();
    console.log("✅ SignalR bağlantısı başarılı.");
    await connection.invoke("JoinChatRoom", chatRoomId); // Odaya katılma
  } catch (error) {
    console.error("❌ SignalR bağlantı hatası:", error);
  }
};

export const sendMessageViaSignalR = async (message: {
  messageId: string;
  userId: string;
  chatRoomId: string;
  content: string;
  mediaUrl: string;
  createdAt: string;
}) => {
  // Mesaj ID'sini gönderilmiş olarak işaretle
  sentMessageIds.add(message.messageId);

  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    try {
      await connection.invoke("SendMessageToRoom", message.chatRoomId, message);
      console.log("📤 Mesaj SignalR üzerinden gönderildi:", message);
    } catch (error) {
      console.error("❌ Mesaj gönderimi sırasında hata:", error);
      // Hata durumunda Set'ten kaldır
      sentMessageIds.delete(message.messageId);
    }
  } else {
    console.warn("⚠️ SignalR bağlı değil, mesaj gönderilemedi.");
    // Bağlantı olmadığında Set'ten kaldır
    sentMessageIds.delete(message.messageId);
  }
};

export const stopSignalRConnection = async () => {
  if (connection) {
    try {
      await connection.stop();
      console.log("🛑 SignalR bağlantısı durduruldu.");
    } catch (error) {
      console.error("❌ SignalR bağlantısı durdurulurken hata:", error);
    }
  }
};