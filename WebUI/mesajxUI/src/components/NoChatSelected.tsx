import { MessageSquare, UserPlus, Users } from "lucide-react";
import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { fetchChatRooms } from "../features/chat/chatRoomSlice";
import CreateChatRoom from "./CreateChatRoom"; // Adjust path to your CreateChatRoom component

const NoChatSelected = () => {
  const dispatch = useAppDispatch();
  const { user } = useAppSelector((state) => state.account);
  const { rooms, error } = useAppSelector((state) => state.chatRoom);
  const [hasRooms, setHasRooms] = useState<boolean | null>(null);
  const [isModalOpen, setIsModalOpen] = useState(false); // Modal state

  useEffect(() => {
    if (user) {
      dispatch(fetchChatRooms(user.userId));
    }
  }, [dispatch, user]);

  useEffect(() => {
    if (rooms) {
      setHasRooms(rooms.length > 0);
    }
  }, [rooms]);

  if (error) return <div>Error: {error}</div>;

  const handleCreateGroup = () => {
    setIsModalOpen(true); // Open the modal
  };

  const handleAddFriend = () => {
    console.log("Arkadaş ekleme işlemi başlatıldı");
  };

  const handleCloseModal = () => {
    setIsModalOpen(false); // Close the modal
  };

  return (
    <div className="w-full flex flex-1 flex-col items-center justify-center p-16 bg-base-100/50">
      <div className="max-w-md text-center space-y-6">
        <div className="flex justify-center gap-4 mb-4">
          <div className="relative">
            <div className="w-16 h-16 rounded-2xl bg-primary/10 flex items-center justify-center animate-bounce">
              <MessageSquare className="w-8 h-8 text-primary" />
            </div>
          </div>
        </div>

        <h2 className="text-2xl font-bold">Welcome to MesajX!</h2>
        <p className="text-base-content/60">
          {hasRooms
            ? "Select a conversation from the sidebar to start chatting"
            : "You don't have any groups yet. Create a group or add a friend to start chatting!"}
        </p>

        {!hasRooms && (
          <div className="flex justify-center gap-4 mt-6">
            <button
              onClick={handleCreateGroup}
              className="btn btn-primary flex items-center gap-2"
            >
              <Users className="w-5 h-5" />
              Create Group
            </button>
            <button
              onClick={handleAddFriend}
              className="btn btn-secondary flex items-center gap-2"
            >
              <UserPlus className="w-5 h-5" />
              Add Friend
            </button>
          </div>
        )}
      </div>

      {/* CreateChatRoom Modal */}
      <CreateChatRoom isOpen={isModalOpen} onClose={handleCloseModal} />
    </div>
  );
};

export default NoChatSelected;