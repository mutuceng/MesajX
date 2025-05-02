import React, { useState, useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { fetchChatRooms } from "../features/chat/chatRoomSlice";
import CreateChatRoom from "./CreateChatRoom";
import { setSelectedChatRoomId } from "../features/chat/messageSlice";

const API_BASE_URL = "http://localhost:5281";

const Sidebar = () => {
  const [isExpanded, setIsExpanded] = useState(false);
  const [selectedChatRoom, setSelectedChatRoom] = useState<any | null>(null);

  const dispatch = useAppDispatch();
  const userId = useAppSelector((state) => state.account.user?.userId);
  const chatRooms = useAppSelector((state) => state.chatRoom.rooms);
  const chatRoomStatus = useAppSelector((state) => state.chatRoom.status);
  const chatRoomError = useAppSelector((state) => state.chatRoom.error);
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  
  const handleCreateGroup = () => {
    setIsModalOpen(true); // Open the modal
  };

  const handleCloseModal = () => {
    setIsModalOpen(false); // Close the modal
  };

  useEffect(() => {
    if (userId && chatRoomStatus === "idle") {
      dispatch(fetchChatRooms(userId));
    }
  }, [userId, chatRoomStatus, dispatch]);

  // Chat rooms'u konsola yazdır
  useEffect(() => {
    console.log("Chat Rooms:", chatRooms);
  }, [chatRooms]);
  

  const handleRoomSelect = (chatRoom: any) => {
    console.log("Butona tıklandı, seçilen chatRoomId:", chatRoom.chatRoomId); // Tıklama logu
    dispatch(setSelectedChatRoomId(chatRoom.chatRoomId));
  };

  return (
    <aside
      className={`h-full w-20 ${isExpanded ? "lg:w-72" : "lg:w-20"} border-r border-base-300 flex flex-col transition-all duration-200`}
      onMouseEnter={() => setIsExpanded(true)}
      onMouseLeave={() => setIsExpanded(false)}
    >
      <div className="border-b border-base-300 w-full p-5">
        <div className="flex items-center gap-2">
          <span className={`font-medium ${isExpanded ? "block" : "hidden"}`}>Groups</span>
        </div>
      </div>

      <div className="overflow-y-auto w-full py-3">
        {chatRoomStatus === "loading" && (
          <p className="text-center text-gray-500">Loading...</p>
        )}
        {chatRoomError && (
          <p className="text-center text-red-500">Error: {chatRoomError}</p>
        )}
        {chatRoomStatus === "succeeded" && chatRooms.length === 0 && (
          <p className="text-center text-gray-500">No groups found.</p>
        )}
        {chatRooms.map((chatRoom) => (
          <button
            key={chatRoom.chatRoomId}
            onClick={() => handleRoomSelect(chatRoom)}
            className={`
              w-full p-3 flex items-center gap-3
              hover:bg-base-200 transition-colors
              ${selectedChatRoom?.chatRoomId === chatRoom.chatRoomId ? "bg-base-300 ring-1 ring-base-300" : ""}
            `}
          >
            <div className="relative mx-auto lg:mx-0">
              <img
                src={`${API_BASE_URL}${chatRoom.photo}` }
                className="size-12 object-cover rounded-full"
              />
              <span
                className="absolute bottom-0 right-0 size-3 bg-green-500 
                rounded-full ring-2 ring-zinc-900"
              />
            </div>

            <div className={`text-left min-w-0 ${isExpanded ? "block" : "hidden"}`}>
              <div className="font-medium truncate">{chatRoom.name}</div>
              <div className="text-sm text-green-400"></div>
            </div>
          </button>
        ))}
      </div>

      <div className="border-t border-base-300 w-full p-5 mt-auto">
        <button
          onClick={handleCreateGroup}
          className="w-full p-3 bg-blue-500 text-white font-medium rounded-lg hover:bg-blue-600 transition-colors"
        >
          {isExpanded ? "Create Group" : "+"}
        </button>
      </div>
      
      <CreateChatRoom isOpen={isModalOpen} onClose={handleCloseModal} />
    </aside>

    
  );
};

export default Sidebar;