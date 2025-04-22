using Microsoft.AspNetCore.SignalR;

namespace SignalRRealTimeAPI.Hubs
{
    public class ChatHub:Hub
    {
        public async Task SendMessageAsync(string chatRoomId, string userId, string message, string? mediaUrl = null)
        {
            await Clients.Group(chatRoomId).SendAsync("ReceiveMessage", new
            {
                ChatRoomId = chatRoomId,
                UserId = userId,
                Content = message,
                MediaUrl = mediaUrl,
                SentAt = DateTime.UtcNow
            });
        }
        public async Task JoinGroupChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveGroupChat(string roomId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        //public override async Task OnConnectedAsync()
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var chatRoomId = httpContext?.Request.Query["chatRoomId"];

        //    if (!string.IsNullOrEmpty(chatRoomId))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoomId);
        //    }

        //    await base.OnConnectedAsync();
        //}

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var httpContext = Context.GetHttpContext();
        //    var chatRoomId = httpContext?.Request.Query["chatRoomId"];

        //    if (!string.IsNullOrEmpty(chatRoomId))
        //    {
        //        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatRoomId);
        //    }

        //    await base.OnDisconnectedAsync(exception);
        //}


    }
}
