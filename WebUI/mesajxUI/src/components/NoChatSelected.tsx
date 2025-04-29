import { MessageSquare, UserPlus, Users } from "lucide-react";
import { useEffect, useState } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import requests from "../api/requests";
import { setUserInfo } from "../features/userInfo/userInfoSlice";

const NoChatSelected = () => {
  
  const dispatch = useAppDispatch();
  const { user } = useAppSelector((state) => state.account);
  const { userInfo } = useAppSelector((state) => state.userInfo);
  const [hasRooms, setHasRooms] = useState<boolean | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserInfoAndRooms = async () => {
      if (!user) {
        setLoading(false);
        return;
      }

      try {
        // Kullanıcı bilgilerini al
        const rooms = await requests.ChatRoom.getRoomsByUserId(user?.userId);
        setHasRooms(rooms.length > 0);
        setLoading(false);
      } catch (error) {
        console.error("Hata oluştu:", error);
        setHasRooms(false);
      } finally {
        setLoading(false);
      }
    };

    fetchUserInfoAndRooms();
  }, [dispatch, user, userInfo]);


  const handleCreateGroup = () => {
    console.log("Grup oluşturma işlemi başlatıldı");
  };

  // Arkadaş ekleme aksiyonu
  const handleAddFriend = () => {
    console.log("Arkadaş ekleme işlemi başlatıldı");
  };

  if (loading) {
    return (
      <div className="w-full flex flex-1 flex-col items-center justify-center p-16 bg-base-100/50">
        <p className="text-base-content/60">Yükleniyor...</p>
      </div>
    );
  }

  return (
<div className="w-full flex flex-1 flex-col items-center justify-center p-16 bg-base-100/50">
      <div className="max-w-md text-center space-y-6">
        <div className="flex justify-center gap-4 mb-4">
          <div className="relative">
            <div
              className="w-16 h-16 rounded-2xl bg-primary/10 flex items-center justify-center animate-bounce"
            >
              <MessageSquare className="w-8 h-8 text-primary" />
            </div>
          </div>
        </div>

        <h2 className="text-2xl font-bold">Welcome to Chatty!</h2>
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
    </div>
  );
};

export default NoChatSelected;