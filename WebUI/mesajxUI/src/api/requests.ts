import axios , { AxiosError, AxiosResponse } from 'axios';
import toast from 'react-hot-toast';
import { store } from '../store/store';

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

axios.interceptors.response.use( response => {
    return response;
}, (error: AxiosError) => {
        const {data, status} = error.response as AxiosResponse;
        switch (status){
            case 400:
                toast.error("Bad Request: " + data.title);
                break;
            case 401:
                toast.error("Unauthorized: " + data.title);
                break;
            case 403:
                toast.error("Forbidden: " + data.title);
                break;
            case 404:
                toast.error("Not Found: " + data.title);
                break;
            case 500:
                toast.error("Internal Server Error: " + data.title);
                break;
            default:
                console.log("Unknown Error: ", data);
        }
        return Promise.reject(error); // hata aldıktan sonra da devam etmesini sağlıyoruz.
    })

const queries = 
{
    getAll: (url: string) => axios.get(url).then((response: AxiosResponse) => response.data),
    getById: (url: string, id: number) => axios.get(`${url}/${id}`).then((response: AxiosResponse) => response.data),
    post: (url: string, body: {}) => axios.post(url, body).then((response: AxiosResponse) => response.data),
    postFormData: (url: string, formData: FormData) =>
        axios.post(url, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
            },
        }).then((response: AxiosResponse) => response.data),
    put: (url: string, body: {}) => axios.put(url, body).then((response: AxiosResponse) => response.data),
    delete: (url: string, id: number) => axios.delete(`${url}/${id}`).then((response: AxiosResponse) => response.data),
}

const Message = 
{
    getAllMessages: () => queries.getAll("chat/Messages"),
    sendMessage: (SendMessageDto: {}) => queries.post("chat/Messages", SendMessageDto),
}

const Account = 
{
    login: (formData: any) => queries.post("user/Auth/login", formData),
    register: (formData: any) => queries.post("user/registers", formData),
    refreshToken: (refreshToken: string) =>
        queries.post("user/Auth/getRefreshToken", { 
            RefreshToken: refreshToken 
        }),
      
}

const ChatRoom = 
{
    getRoomsByUserId: (userId: string) => queries.getAll(`chat/ChatRooms/${userId}`),
    getChatRoomById: (id: number) => queries.getById("chat/ChatRooms", id),
    createChatRoom: (formData: FormData) => queries.postFormData("chat/ChatRooms", formData),
    updateChatRoom: (UpdateChatRoomDto: {}) => queries.put("chat/ChatRooms", UpdateChatRoomDto),
    deleteChatRoom: (id: number) => queries.delete("chat/ChatRooms", id),
}

const ChatRoomMember = 
{
    addMember: (AddMemberDto: {}) => queries.post("chat/ChatRoomMembers/addMember", AddMemberDto),
    removeMember: (id: number) => queries.delete("chat/RoomMembers/removeMember", id),
    getMembersByRoomId: (roomId: number) => queries.getAll(`chat/RoomMembers/${roomId}`),
}

const requests =
{
    Message,
    Account,
    ChatRoom,
    ChatRoomMember,
}

export default requests;