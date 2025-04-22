import Chat from "../components/chat/Chat";
import Sidebar from "../components/sidebar/Sidebar";
import NoChatSelected from "../components/NoChatSelected";
import { useState } from "react";


function HomePage() {

  const [showChat, setShowChat] = useState(true);

  const toggleChat = () => {
    setShowChat((prev) => !prev);
  };
  
  return (

    
    <div className="h-screen bg-base-200">
      <div className="flex items-center justify-center pt-20 px-4">
        <div className="bg-base-100 rounded-lg shadow-cl w-full max-w-6xl h-[calc(100vh-8rem)]">
            <div className="flex h-full rounded-lg overflow-hidden">
              <Sidebar />
              <div className="flex-1 relative">
              {/* Toggle butonu */}
              <button
                className="absolute top-4 right-4 z-10 btn btn-primary"
                onClick={toggleChat}
              >
                {showChat ? "NoChatSelected Göster" : "Chat Göster"}
              </button>

              {/* İçerik */}
              {showChat ? <Chat /> : <NoChatSelected />}
            </div>
            </div>
        </div>
    
      </div>
    </div>
  )
}

export default HomePage


