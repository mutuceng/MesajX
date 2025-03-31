import React, { useState } from "react";
import "./Sidebar.css";

const users = [
  { id: 1, name: "Jon Snow", img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fpreview.redd.it%2Farya-created-by-dall-e-3-based-on-the-description-at-v0-4nseaay5iytb1.png%3Fwidth%3D640%26crop%3Dsmart%26auto%3Dwebp%26s%3D6c5df7383ab7ebb13c661b7496448f7a07b2ed2e&f=1&nofb=1&ipt=e4acd4d2dd849c0719ebbbeac083e1a86e5d37d8d040ed972204c9718b6277da&ipo=images" },
  { id: 2, name: "Arya Stark", img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.pinimg.com%2F736x%2F5c%2F4d%2Fc6%2F5c4dc6e07ab55132f76d697d043badb1.jpg&f=1&nofb=1&ipt=a6cce5824d1d8f4e822037a7d213ac3e2ef59bf46e43d1569aa94cbd1a96175c&ipo=images" },
  { id: 3, name: "Robb Stark", img: "https://external-content.duckduckgo.com/iu/?u=http%3A%2F%2Fawoiaf.westeros.org%2Fimages%2Ff%2Ffc%2FRobb_Stark_HBO.jpg&f=1&nofb=1&ipt=366ac1ee0569b151ae546cd61fd343bce854477d11c31b0d482eb2e82c7550a2&ipo=images" },
  { id: 4, name: "Tyrion Lannister", img: "https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fcomicvine.gamespot.com%2Fa%2Fuploads%2Fsquare_small%2F11157%2F111572510%2F8509264-795321.jpg&f=1&nofb=1&ipt=00a379ae5286d036a0d83a957e0a0b531be688efe0c973f9d2d6c58c8dbc7c79&ipo=images" },

];

const Sidebar: React.FC = () => {
    const [isExpanded, setIsExpanded] = useState(false);
  
    return (
      <div
        className={`sidebar ${isExpanded ? "expanded" : ""}`}
        onMouseEnter={() => setIsExpanded(true)}
        onMouseLeave={() => setIsExpanded(false)}
      >
        
        <div className="user-list">
          {users.map((user) => (
                <div key={user.id} className="user-item">
                    <img src={user.img} alt={user.name} className="user-avatar" />
                    <span className={`user-name ${isExpanded ? "visible" : ""}`}>
                        {user.name}
                    </span>
                </div>
            ))}
        </div>
      </div>
    );
  };

export default Sidebar;
