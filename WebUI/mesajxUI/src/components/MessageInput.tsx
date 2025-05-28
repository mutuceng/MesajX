import { useState, useRef } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { sendMessage } from "../features/chat/messageSlice";
import { sendMessageViaSignalR } from "../api/SignalRService";
import EmojiPicker from "emoji-picker-react";
import { Smile, Send } from "lucide-react";
import { v4 as uuidv4 } from "uuid";

const MessageInput = () => {
  const dispatch = useAppDispatch();
  const userId = useAppSelector((state) => state.account.user?.userId);
  const chatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);
  const [content, setContent] = useState("");
  const [showEmojiPicker, setShowEmojiPicker] = useState(false);
  const textareaRef = useRef<HTMLTextAreaElement>(null);

  const adjustTextareaHeight = () => {
    const textarea = textareaRef.current;
    if (textarea) {
      textarea.style.height = "auto";
      textarea.style.height = `${Math.min(textarea.scrollHeight, 120)}px`;
    }
  };

   const handleSend = async () => {
    if (!content.trim() || !userId || !chatRoomId) return;

    const messageToSend = {
      messageId: uuidv4(),
      userId,
      chatRoomId,
      content: content.trim(),
      mediaUrl: "",
      createdAt: new Date().toISOString(),
    };

    try {
      // Önce yerel store'a ekle - kullanıcıya anında geri bildirim sağlar
      dispatch(sendMessage(messageToSend));
      
      // Sonra SignalR üzerinden gönder
      await sendMessageViaSignalR(messageToSend);
      
      console.log("✅ Mesaj gönderildi:", messageToSend);
      setContent("");
      if (textareaRef.current) {
        textareaRef.current.style.height = "auto";
      }
    } catch (error) {
      console.error("❌ Mesaj gönderme hatası:", error);
      // Hata durumunda mesajı kaldırmak isteyebilirsiniz (opsiyonel)
    }
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLTextAreaElement>) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  const handleEmojiClick = (emojiData: any) => {
    setContent((prev) => prev + emojiData.emoji);
    setShowEmojiPicker(false);
  };

  return (
    <div className="p-4 border-t bg-gradient-to-r from-gray-800 to-gray-700 shadow-md">
      <div className="relative flex items-center gap-3">
        {/* Emoji Butonu */}
        <button
          onClick={() => setShowEmojiPicker((prev) => !prev)}
          className="text-gray-300 hover:text-white transition-colors duration-200"
          aria-label="Emoji seç"
        >
          <Smile className="w-6 h-6" />
        </button>

        {/* Emoji Picker */}
        {showEmojiPicker && (
          <div className="absolute bottom-16 left-0 z-50 sm:bottom-20 sm:left-2">
            <EmojiPicker onEmojiClick={handleEmojiClick} width={300} height={400} />
          </div>
        )}

        {/* Mesaj Textarea */}
        <textarea
          ref={textareaRef}
          className="flex-1 px-4 py-2 bg-gray-900 border border-gray-600 rounded-2xl focus:outline-none focus:ring-2 focus:ring-blue-500 resize-none overflow-y-auto placeholder-gray-400"
          placeholder="Mesaj yaz..."
          value={content}
          onChange={(e) => {
            setContent(e.target.value);
            adjustTextareaHeight();
          }}
          onKeyDown={handleKeyDown}
          rows={1}
          style={{ minHeight: "40px", maxHeight: "120px" }}
        />

        {/* Gönder Butonu */}
        <button
          onClick={handleSend}
          disabled={!content.trim()}
          className={`p-2 rounded-full transition-colors duration-200 focus:outline-none focus:ring-2 focus:ring-blue-500 ${
            content.trim()
              ? "bg-blue-500 text-white hover:bg-blue-600"
              : "bg-gray-500 text-gray-300 cursor-not-allowed"
          }`}
          aria-label="Mesaj gönder"
        >
          <Send className="w-6 h-6" />
        </button>
      </div>
    </div>
  );
};

export default MessageInput;
