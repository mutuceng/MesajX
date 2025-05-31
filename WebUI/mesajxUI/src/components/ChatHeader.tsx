import { X, MoreVertical } from "lucide-react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import requests from "../api/requests";
import { setSelectedRoom } from "../features/chat/chatRoomSlice";
import AddMember from "./AddMember";
import { store } from "../store/store";
import { resetMessages } from "../features/chat/messageSlice";

const API_BASE_URL = "http://localhost:5281";

interface ChatRoom {
  [x: string]: any;
  chatRoomId: string;
  name: string;
  photoPath: string;
}
const ChatHeader = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const selectedChatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);
  const [roomInfo, setRoomInfo] = useState<ChatRoom | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [isDropdownOpen, setIsDropdownOpen] = useState(false); // Dropdown durumu
  const dropdownRef = useRef<HTMLDivElement>(null);

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

  // Dropdown dışına tıklama kontrolü
  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
        setIsDropdownOpen(false);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, []);

  const handleClose = () => {
    console.log("Close butonuna tıklandı, seçili oda ID:", selectedChatRoomId);
    dispatch(setSelectedRoom(null));
    setRoomInfo(null);
    dispatch(resetMessages());
    setRoomInfo(null);
    setIsDropdownOpen(false);
    navigate("/");
  };

  const handleAddMember = () => {
    setIsModalOpen(true);
  };

const handleLeaveGroup = async () => {
  if (!selectedChatRoomId) return;
  const userId = store.getState().account.user?.userId; // userId'yi store'dan al
  if (!userId) {
    setError("Kullanıcı ID'si bulunamadı");
    return;
  }
  try {
    await requests.ChatRoomMember.removeMember(userId, selectedChatRoomId);
    console.log("Gruptan başarıyla ayrıldınız:", selectedChatRoomId);
    dispatch(setSelectedRoom(null));
    setRoomInfo(null);
    setIsDropdownOpen(false);
  } catch (err: any) {
    console.error("Gruptan ayrılırken hata:", err);
    setError(err.message || "Gruptan ayrılma başarısız");
  }
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
                  onError={(e: React.SyntheticEvent<HTMLImageElement, Event>) => {
                    e.currentTarget.src = "/avatar.png";
                    e.currentTarget.onerror = null;
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
        <div className="flex items-center gap-2 relative" ref={dropdownRef}>
          <button
            onClick={handleAddMember}
            className="btn btn-sm btn-primary text-white flex items-center gap-1"
          >
            <span role="img" aria-label="add">👥</span> Add Member
          </button>
          <button
            onClick={handleClose}
            className="btn btn-sm btn-error text-white flex items-center gap-1 hover:bg-red-600 transition-colors"
          >
            <X size={16} /> Close
          </button>
          <button
            onClick={() => setIsDropdownOpen(!isDropdownOpen)}
            className="btn btn-sm btn-ghost text-white flex items-center gap-1"
          >
            <MoreVertical size={16} />
          </button>
          {isDropdownOpen && (
            <div className="absolute right-0 top-10 mt-2 w-48 bg-base-100 border border-base-300 rounded-md shadow-lg z-10">
              <ul className="py-1">
                <li>
                  <button
                    onClick={handleLeaveGroup}
                    className="block w-full text-left px-4 py-2 text-sm text-base-content hover:bg-base-200"
                  >
                    Leave
                  </button>
                </li>
              </ul>
            </div>
          )}
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