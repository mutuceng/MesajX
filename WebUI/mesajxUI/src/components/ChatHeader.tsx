import { X } from "lucide-react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { useEffect, useState } from "react";
import requests from "../api/requests";
import { setSelectedRoom } from "../features/chat/chatRoomSlice";
import { clearMessages } from "../features/chat/messageSlice";
import AddMember from "./AddMember";

const API_BASE_URL = "http://localhost:5281";

interface ChatRoom {
  [x: string]: any;
  chatRoomId: string;
  name: string;
  photoPath: string;
}

const ChatHeader = () => {
  const dispatch = useAppDispatch();
  const selectedChatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);
  const [roomInfo, setRoomInfo] = useState<ChatRoom | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false); // Modal durumu

  // Backend'den oda bilgisini çek
  useEffect(() => {
    console.log("Header Selected chat room ID:", selectedChatRoomId);

    const fetchRoomInfo = async () => {
      if (!selectedChatRoomId) {
        setRoomInfo(null);
        return;
      }

      setLoading(true);
      setError(null);

      try {
        const response = await requests.ChatRoom.getChatRoomById(selectedChatRoomId);
        console.log("Backend'den alınan oda bilgisi:", response);
        setRoomInfo(response);
        dispatch(setSelectedRoom(response));
      } catch (err: any) {
        setError(err.message || "Oda bilgisi alınamadı");
        console.error("Oda bilgisi alınırken hata:", err);
      } finally {
        setLoading(false);
      }
    };

    fetchRoomInfo();
  }, [selectedChatRoomId, dispatch]);

  const handleClose = () => {
    console.log("Close butonuna tıklandı, seçili oda ID:", selectedChatRoomId);
    dispatch(setSelectedRoom(null));
    dispatch(clearMessages());
    setRoomInfo(null);
  };

  const handleAddMember = () => {
    setIsModalOpen(true); // Üye ekleme modalını aç
  };

  return (
    <div className="p-2.5 border-b border-base-300">
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-3">
          {/* Avatar */}
          <div className="avatar">
            <div className="size-10 rounded-full relative">
              {loading ? (
                <div className="size-10 rounded-full bg-gray-200 animate-pulse" />
              ) : (
                <img
                  src={
                    roomInfo?.photo
                      ? `${API_BASE_URL}${roomInfo.photo}`
                      : "/avatar.png"
                  }
                  alt={roomInfo?.name || "User"}
                  onError={(e) => {
                    e.currentTarget.src = "/avatar.png";
                  }}
                />
              )}
            </div>
          </div>

          {/* User info */}
          <div>
            {loading ? (
              <div className="h-5 w-20 bg-gray-200 rounded animate-pulse" />
            ) : (
              <h3 className="font-medium">{roomInfo?.name || "No Room Selected"}</h3>
            )}
            <p className="text-sm text-base-content/70">
              {error ? "Error" : roomInfo ? "Online" : "Offline"}
            </p>
          </div>
        </div>

        {/* Butonlar */}
        <div className="flex items-center gap-2">
          <button
            onClick={handleAddMember}
            className="btn btn-sm btn-primary text-white flex items-center gap-1"
          >
            <span role="img" aria-label="add">👥</span> Üye Ekle
          </button>
          <button
            onClick={handleClose}
            className="btn btn-sm btn-error text-white flex items-center gap-1 hover:bg-red-600 transition-colors"
          >
            <X size={16} /> Kapat
          </button>
        </div>
      </div>

      {/* Üye Ekle Modalı */}
      <AddMember
        isOpen={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        chatRoomId={selectedChatRoomId}
      />
    </div>
  );
};

export default ChatHeader;