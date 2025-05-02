import React, { useState, useEffect } from 'react';
import Modal from 'react-modal';
import { useAppDispatch, useAppSelector } from '../hooks/hooks';
import { createChatRoom, clearCreatedPhotoPath } from '../features/chat/chatRoomSlice';
import { addMember, resetMemberState } from '../features/chat/chatRoomMemberSlice';

const API_BASE_URL = "http://localhost:5281";

interface CreateChatRoomProps {
  isOpen: boolean;
  onClose: () => void;
}

const CreateChatRoom: React.FC<CreateChatRoomProps> = ({ isOpen, onClose }) => {
  const [groupName, setGroupName] = useState('');
  const [photo, setPhoto] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);

  const dispatch = useAppDispatch();
  const { status: chatRoomStatus, error: chatRoomError, createdPhotoPath } = useAppSelector(
    (state) => state.chatRoom
  );
  const { loading: memberLoading, error: memberError, success: memberSuccess } = useAppSelector(
    (state) => state.chatRoomMember
  );

  const userId = useAppSelector((state) => state.account.user?.userId);

  useEffect(() => {
    if (!isOpen) {
      dispatch(clearCreatedPhotoPath());
      dispatch(resetMemberState());
      setGroupName('');
      setPhoto(null);
      setPreviewUrl(null);
    }
  }, [isOpen, dispatch]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!groupName.trim()) {
      return;
    }

    const groupData = { name: groupName, photo: photo || undefined };

    try {
      // Grubu oluştur
      const createResponse = await dispatch(createChatRoom(groupData)).unwrap();
      const chatRoomId = createResponse.chatRoomId; 
      if (!chatRoomId) {
        throw new Error("Grup ID'si alınamadı.");
      }

      console.log('Grubu oluşturan:', userId + ' - name ID:', groupData.name);

      // Grubu oluşturan kullanıcıyı gruba üye olarak ekle
      await dispatch(
        addMember({
          userId: userId ?? '', 
          chatRoomId: chatRoomId,
          role: 0, // Kurucu rolü
        })
      ).unwrap();

      setGroupName('');
      setPhoto(null);
      setPreviewUrl(null);
      onClose();
    } catch (err) {
      console.error('Hata:', err);
    }
  };

  const handlePhotoChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const file = e.target.files?.[0];
    if (file) {
      setPhoto(file);
      setPreviewUrl(URL.createObjectURL(file));
    }
  };

  return (
    <Modal
      isOpen={isOpen}
      onRequestClose={onClose}
      ariaHideApp={false}
      className="fixed inset-0 flex items-center justify-center z-50"
      overlayClassName="fixed inset-0 bg-black bg-opacity-50 z-40 backdrop-blur-sm"
    >
      <div className="bg-white text-gray-900 rounded-2xl shadow-2xl w-full max-w-md p-6">
        <h2 className="text-2xl font-bold mb-6 text-center text-gray-800">
          Yeni Sohbet Odası Oluştur
        </h2>
        <form onSubmit={handleSubmit} className="space-y-5">
          <div>
            <label htmlFor="groupName" className="block font-semibold text-sm mb-1 text-gray-700">
              Grup Adı
            </label>
            <input
              id="groupName"
              type="text"
              value={groupName}
              onChange={(e) => setGroupName(e.target.value)}
              placeholder="Grup adını girin"
              className="w-full border border-gray-300 rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500 bg-white text-gray-900"
              required
            />
          </div>

          <div>
            <label htmlFor="photo" className="block font-semibold text-sm mb-1 text-gray-700">
              Grup Fotoğrafı (isteğe bağlı)
            </label>
            <input
              id="photo"
              type="file"
              accept="image/*"
              onChange={handlePhotoChange}
              className="block w-full text-sm text-gray-600 file:mr-4 file:py-2 file:px-4
                        file:rounded-md file:border-0
                        file:text-sm file:font-semibold
                        file:bg-blue-100 file:text-blue-700
                        hover:file:bg-blue-200"
            />
            {previewUrl && (
              <img
                src={previewUrl}
                alt="Önizleme"
                className="mt-3 h-24 w-24 object-cover rounded-md border border-gray-300"
              />
            )}
          </div>

          {createdPhotoPath && (
            <div>
              <p className="text-sm text-green-600">
                Görsel: {createdPhotoPath.includes('default.png') ? 'Varsayılan görsel kullanıldı' : 'Görsel yüklendi'}
              </p>
              <img
                src={`${API_BASE_URL}${createdPhotoPath}`}
                alt="Kaydedilen Görsel"
                crossOrigin="anonymous"
                className="mt-3 h-24 w-24 object-cover rounded-md border border-gray-300"
                onError={(e) => {
                  console.error("Görsel yükleme hatası:", e);
                  e.currentTarget.src = `${API_BASE_URL}/uploads/default.png`;
                }}
              />
            </div>
          )}

          {(chatRoomStatus === 'loading' || memberLoading) && (
            <p className="text-sm text-gray-500">Yükleniyor...</p>
          )}
          {chatRoomError && (
            <p className="text-sm text-red-600 font-medium">Hata: {chatRoomError}</p>
          )}
          {memberError && (
            <p className="text-sm text-red-600 font-medium">Üye Ekleme Hatası: {memberError}</p>
          )}
          {memberSuccess && (
            <p className="text-sm text-green-600 font-medium">Kurucu üye olarak eklendi!</p>
          )}

          <div className="flex justify-end gap-3 pt-2">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 rounded-md bg-gray-200 hover:bg-gray-300 text-gray-800 font-medium"
              disabled={chatRoomStatus === 'loading' || memberLoading}
            >
              İptal
            </button>
            <button
              type="submit"
              disabled={chatRoomStatus === 'loading' || memberLoading}
              className="px-4 py-2 rounded-md bg-blue-600 hover:bg-blue-700 text-white font-semibold disabled:opacity-60"
            >
              Oluştur
            </button>
          </div>
        </form>
      </div>
    </Modal>
  );
};

export default CreateChatRoom;