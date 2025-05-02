import React, { useState } from "react";
import EmojiPicker from "emoji-picker-react";

interface MessageInputProps {
  onSend: (message: string) => void;
}

const MessageInput: React.FC<MessageInputProps> = ({ onSend }) => {
  const [input, setInput] = useState("");
  const [showEmojiPicker, setShowEmojiPicker] = useState(false);

  const handleSend = () => {
    if (input.trim()) {
      onSend(input);
      setInput("");
      setShowEmojiPicker(false);
    }
  };

  const handleEmojiClick = (emojiData: any) => {
    setInput((prev) => prev + emojiData.emoji);
    setShowEmojiPicker(false);
  };

  return (
    <div className="message-input flex items-center bg-gray-600 p-2 rounded-lg shadow-md w-full max-w-2xl mx-auto relative">
      {/* Emoji Butonu */}
      <button
        className="emoji-button text-xl p-2 hover:bg-gray-200 rounded-full focus:outline-none focus:ring-2 focus:ring-blue-500"
        onClick={() => setShowEmojiPicker(!showEmojiPicker)}
        aria-label="Emoji picker aç/kapat"
      >
        😊
      </button>
      

      {/* Emoji Picker */}
      {showEmojiPicker && (
        <div className="absolute bottom-16 z-10">
          <EmojiPicker onEmojiClick={handleEmojiClick} />
        </div>
      )}

      {/* Mesaj Input */}
      <input
        type="text"
        placeholder="Mesaj yaz..."
        value={input}
        onChange={(e) => setInput(e.target.value)}
        onKeyDown={(e) => e.key === "Enter" && handleSend()}
        className="flex-1 p-2 ml-2 bg-gray-800 text-white border border-gray-600 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 text-sm placeholder-gray-400"
      />

      {/* Gönder Butonu */}
      <button
        className="send-button bg-blue-500 text-white p-2 ml-2 rounded-full hover:bg-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:bg-gray-400 disabled:cursor-not-allowed"
        onClick={handleSend}
        disabled={!input.trim()}
        aria-label="Mesajı gönder"
      >
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          stroke="currentColor"
          className="w-6 h-6"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            strokeWidth={2}
            d="M12 19l9 2-9-18-9 18 9-2zm0 0v-8"
          />
        </svg>
      </button>
    </div>
  );
};

export default MessageInput;
