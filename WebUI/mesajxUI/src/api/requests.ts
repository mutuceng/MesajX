import axios , { AxiosError, AxiosResponse } from 'axios';
import toast from 'react-hot-toast';
import { store } from '../store/store';
import { UserInfo } from '../constants/types/IUserInfo';

axios.defaults.baseURL = "http://localhost:5281/api/";
axios.defaults.withCredentials = true;
axios.defaults.headers.post['Content-Type'] = 'application/json';


axios.interceptors.request.use( request => {

    console.log("Request:  authmiddleware calıstı", request);
    const token = store.getState().account.user?.accessToken;
    console.log("Store State:", store.getState());
    if(token){
        console.log("token:  token okey", token);
        request.headers.Authorization = `Bearer ${token}`;
    }
    else{
        console.log("token:  token yok", token);
    }
    return request;
})

axios.interceptors.response.use(
    response => response,
    (error: AxiosError) => {
      const { data, status } = error.response as AxiosResponse;
      const errorMessage = data?.message || data?.title || "Bilinmeyen bir hata oluştu";
      const errorDetails = data?.errors || data?.detail || "";
      
      switch (status) {
        case 400:
          toast.error(`Hatalı istek: ${errorMessage} ${errorDetails ? `\nDetaylar: ${errorDetails}` : ''}`);
          break;
        case 401:
          toast.error(`Giriş yapmanız gerekiyor: ${errorMessage}`);
          break;
        case 403:
          toast.error(`Bu işlemi yapmaya yetkiniz yok: ${errorMessage}`);
          break;
        case 404:
          toast.error(`Kaynak bulunamadı: ${errorMessage}`);
          break;
        case 500:
          toast.error(`Sunucu hatası: ${errorMessage}`);
          break;
        default:
          toast.error(`Bilinmeyen bir hata: ${errorMessage}`);
      }
      return Promise.reject(error);
    }
  );


const queries = 
{
    getAll: (url: string) => axios.get(url).then((response: AxiosResponse) => response.data),
    getWithParams: (url: string, params: any) =>
        axios
          .get(url, { params })
          .then((response: AxiosResponse) => response.data),
    getById: (url: string, id: string) => axios.get(`${url}/${id}`).then((response: AxiosResponse) => response.data),
    post: (url: string, body: {}) => axios.post(url, body).then((response: AxiosResponse) => response.data),
    postFormData: (url: string, formData: FormData) =>
        axios.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        }).then((response: AxiosResponse) => response.data),
    put: (url: string, body: {}) => axios.put(url, body).then((response: AxiosResponse) => response.data),
    delete: (url: string, id: number) => axios.delete(`${url}/${id}`).then((response: AxiosResponse) => response.data),
    setDelete: (url: string, params?: { [key: string]: any }) =>
            axios.delete(url, { params }).then((response: AxiosResponse) => response.data),}

const Message = 
{
    getMessagesByRoomId: async (chatRoomId: string, page: number = 1) => {
        try {
            console.log("getMessagesByRoomId isteği gönderiliyor:", { chatRoomId, page });
            const response = await queries.getWithParams("chat/Messages", { chatRoomId, page });
            console.log("getMessagesByRoomId yanıtı:", response);
            return response;
        } catch (error) {
            console.error("getMessagesByRoomId hatası:", error);
            throw error;
        }
    },
    sendMessage: async (sendMessageDto: {}) => {
        try {
            console.log("sendMessage isteği gönderiliyor:", sendMessageDto);
            const response = await queries.post("chat/messages", sendMessageDto);
            console.log("sendMessage yanıtı:", response);
            return response;
        } catch (error) {
            console.error("sendMessage hatası:", error);
            throw error;
        }
    }
}

const Account = 
{
    login: (formData: any) => queries.post("user/Auth/login", formData),
    register: (formData: any) => queries.postFormData("user/registers", formData),
    refreshToken: (refreshToken: string) =>
        queries.post("user/Auth/getRefreshToken", { 
            RefreshToken: refreshToken 
        }),
    getUserIdByUsername : (username: string) =>
        queries.getById("user/users/username", username),
    getUserProfile: () =>
        queries.getAll("user/users/GetUserInfo"),
    getChatUsers: (userIds: string[]): Promise<UserInfo[]> => 
        queries.post("user/users/getchatusers", { userIds }),
    updateUserProfile: (userId: string, updateDto: {}) =>
        queries.put(`user/users/updateprofile?userId=${userId}`, updateDto),
      
}

const ChatRoom = 
{
    getRoomsByUserId: (userId: string) => queries.getAll(`chat/ChatRooms/${userId}`),
    getChatRoomById: (id: string) => queries.getById("chat/ChatRooms/room", id),
    createChatRoom: (formData: FormData) => queries.postFormData("chat/ChatRooms", formData),
    updateChatRoom: (UpdateChatRoomDto: {}) => queries.put("chat/ChatRooms", UpdateChatRoomDto),
    deleteChatRoom: (id: number) => queries.delete("chat/ChatRooms", id),
}

const ChatRoomMember = 
{
    addMember: (AddMemberDto: {}) => queries.post("chat/ChatRoomMembers/addMember", AddMemberDto),
    removeMember: (userId: string, chatRoomId: string) =>
        queries.setDelete("chat/ChatRoomMembers/removeMember", { userId, chatRoomId }),
    getMembersByRoomId: (roomId: number) => queries.getAll(`chat/RoomMembers/${roomId}`),
}

const requests = {
    Message,
    Account,
    ChatRoom,
    ChatRoomMember,
  };

export default requests;