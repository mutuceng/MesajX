import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { fetchUserProfile } from "../features/account/accountSlice";
import { useNavigate } from "react-router-dom";
import { Pencil } from "lucide-react"; // Şık kalem ikonu için lucide-react

const ProfilePage = () => {
  const API_BASE_URL = "http://localhost:5281";
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const userProfile = useAppSelector((state) => state.account.userProfile);

  useEffect(() => {
    dispatch(fetchUserProfile());
  }, [dispatch]);

  if (!userProfile || Object.keys(userProfile).length === 0)
    return <div>Kullanıcı bulunamadı.</div>;

  return (
    <div className="flex flex-col items-center justify-center min-h-screen pt-24 px-4"
         style={{ maxWidth: 400, margin: "0 auto" }}>
      <div className="relative">
        <img
          src={`${API_BASE_URL}${userProfile.profileImageUrl}`}
          alt={`${userProfile.username} profil fotoğrafı`}
          className="w-32 h-32 rounded-full object-cover mb-4"
        />
        {/* <button
          className="absolute bottom-2 right-2 bg-white rounded-full p-1 shadow hover:bg-gray-100"
          onClick={() => navigate("/profile/edit")}
        >
          <Pencil className="w-5 h-5 text-gray-600" />
        </button> */}
      </div>
      <h2 className="text-xl font-bold">{userProfile.username}</h2>
    </div>
  );
};

export default ProfilePage;
