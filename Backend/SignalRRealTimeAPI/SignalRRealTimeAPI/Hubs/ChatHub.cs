using Microsoft.AspNetCore.SignalR;

namespace SignalRRealTimeAPI.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessageAsync(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task JoinGroupChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendPrivateMessage(string chatId, string senderId, string message, string mediaUrl = null)
        {

        }
    }
}
