import { useState, useEffect, useRef } from "react";
import { useAppDispatch, useAppSelector } from "../hooks/hooks";
import { updateUserProfile } from "../features/account/accountSlice";
import { useNavigate } from "react-router-dom";
import { Pencil } from "lucide-react"; // ikon için (lucide-react yüklü değilse yükle: npm i lucide-react)

const API_BASE_URL = "http://localhost:5281";

const EditProfilePage = () => {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();
  const userProfile = useAppSelector((state) => state.account.userProfile);

  const [username, setUsername] = useState(userProfile?.username || "");
  const [image, setImage] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string>("");

  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    if (userProfile?.profileImageUrl) {
      setPreviewUrl(`${API_BASE_URL}${userProfile.profileImageUrl}`);
    }
  }, [userProfile]);

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0] || null;
    setImage(file);
    if (file) {
      const preview = URL.createObjectURL(file);
      setPreviewUrl(preview);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    if (!userProfile?.id) {
      console.error("Kullanıcı ID'si bulunamadı.");
      return;
    }

    const updateDto = {
      userName: username,
      // Profil resmi için endpoint destekliyorsa, base64 veya başka bir format eklenebilir
      // profileImage: image ? await convertFileToBase64(image) : undefined,
    };

    try {
      await dispatch(updateUserProfile({ userId: userProfile.id, updateDto })).unwrap();
      navigate("/profile");
    } catch (error) {
      console.error("Güncelleme hatası:", error);
    }
  };

  // Yardımcı fonksiyon: Dosyayı base64'e çevirmek (profil resmi için gerekirse)
  // const convertFileToBase64 = (file: File): Promise<string> => {
  //   return new Promise((resolve, reject) => {
  //     const reader = new FileReader();
  //     reader.readAsDataURL(file);
  //     reader.onload = () => resolve(reader.result as string);
  //     reader.onerror = (error) => reject(error);
  //   });
  // };

  return (
    <div className="flex flex-col items-center justify-center min-h-screen pt-24 px-4">
      <form onSubmit={handleSubmit} className="space-y-4 w-full max-w-sm">
        <div className="relative w-32 h-32 mx-auto">
          {previewUrl && (
            <img
              src={previewUrl}
              alt="Önizleme"
              className="w-32 h-32 rounded-full object-cover border"
            />
          )}
          {/* İkon ve file input */}
          <div
            className="absolute bottom-0 right-0 border rounded-full p-1 cursor-pointer hover:bg-gray-600"
            onClick={() => fileInputRef.current?.click()}
            title="Fotoğrafı değiştir"
          >
            <Pencil size={16} />
            <input
              type="file"
              accept="image/*"
              onChange={handleFileChange}
              ref={fileInputRef}
              className="hidden"
            />
          </div>
        </div>

        <div>
          <label className="block mb-1 font-medium">Kullanıcı Adı</label>
          <input
            type="text"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            className="w-full border px-3 py-2 rounded"
          />
        </div>

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700 w-full"
        >
          Güncelle
        </button>
      </form>
    </div>
  );
};

export default EditProfilePage;