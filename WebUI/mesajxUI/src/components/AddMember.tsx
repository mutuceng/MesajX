import React, { useState } from "react";
import { useAppDispatch } from "../hooks/hooks";
import { Role } from "../api/types";
import { addMember } from "../features/chat/chatRoomMemberSlice";
import { getUserIdByUsername } from "../features/account/accountSlice";

interface AddMemberModalProps {
  isOpen: boolean;
  onClose: () => void;
  chatRoomId: string | null;
}

const AddMemberModal = ({ isOpen, onClose, chatRoomId }: AddMemberModalProps) => {
  const dispatch = useAppDispatch();
  const [username, setUsername] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [role, setRole] = useState<Role>(Role.Member);
  const [loading, setLoading] = useState(false);

  if (!isOpen) return null;

  const handleAddMember = async () => {
    if (!chatRoomId) {
      setError("Oda seçili değil!");
      return;
    }

    if (!username.trim()) {
      setError("Kullanıcı adı boş olamaz!");
      return;
    }

    setLoading(true);
    setError(null);

    try {
      // Kullanıcı adından UserId al
      const userIdResult = await dispatch(getUserIdByUsername(username)).unwrap();
      if (!userIdResult) {
        throw new Error("Kullanıcı bulunamadı");
      }

      // Üye ekleme işlemi
      await dispatch(
        addMember({
          chatRoomId,
          userId: "userIdResult",
          role: Role.Member, // Varsayılan Role: Member (1)
        })
      ).unwrap();

      setUsername(""); // Formu temizle
      onClose(); // Modalı kapat
    } catch (err: any) {
      setError(err.message || "Üye eklenemedi");
      console.error("Üye ekleme hatası:", err);
    } finally {
      setLoading(false);
    }
  };

  return (
<div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
    <div className="bg-base-100 p-6 rounded-lg shadow-lg w-full max-w-md">
      <h2 className="text-xl font-bold mb-4">Üye Ekle</h2>
      <input
        type="text"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
        placeholder="Kullanıcı adı girin"
        className="w-full p-2 mb-4 border rounded"
        disabled={loading}
      />
      <select
        value={role}
        onChange={(e) => setRole(Number(e.target.value) as Role)}
        className="w-full p-2 mb-4 border rounded"
        disabled={loading}
      >
        <option value={Role.Member}>Member</option>
        <option value={Role.Moderator}>Moderator</option>
      </select>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <div className="flex justify-end gap-2">
        <button
          onClick={onClose}
          className="btn btn-sm btn-secondary"
          disabled={loading}
        >
          İptal
        </button>
        <button
          onClick={handleAddMember}
          className="btn btn-sm btn-primary flex items-center gap-1"
          disabled={loading}
        >
          {loading ? "Ekleniyor..." : <>Ekle <span role="img" aria-label="add">👥</span></>}
        </button>
      </div>
    </div>
  </div>
  );
};

export default AddMemberModal;