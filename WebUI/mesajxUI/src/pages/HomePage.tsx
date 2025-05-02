import Chat from "../components/Chat";
import Sidebar from "../components/Sidebar";
import NoChatSelected from "../components/NoChatSelected";
import { useAppSelector } from "../hooks/hooks";


function HomePage() {
  const selectedChatRoomId = useAppSelector((state) => state.message.selectedChatRoomId);

  return (
    <div className="h-screen bg-base-200">
      <div className="flex items-center justify-center pt-20 px-4">
        <div className="bg-base-100 rounded-lg shadow-cl w-full max-w-6xl h-[calc(100vh-8rem)]">
          <div className="flex h-full rounded-lg overflow-hidden">
            <Sidebar />
            <div className="flex-1 relative">
              {selectedChatRoomId ? <Chat /> : <NoChatSelected />}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default HomePage


