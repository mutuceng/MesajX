using MassTransit;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using Microsoft.AspNetCore.SignalR;

namespace MesajX.ChatService.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessageToRoom(string chatRoomId, SendMessageDto message)
        {
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", message);
        }
        public async Task JoinChatRoom(string chatRoomId)
        {
            // Kullanıcıyı sohbet odasına ekle
            await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        }

        public async Task LeaveChatRoom(string chatRoomId)
        {
            // Kullanıcıyı sohbet odasından çıkar
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        }
    }
}
