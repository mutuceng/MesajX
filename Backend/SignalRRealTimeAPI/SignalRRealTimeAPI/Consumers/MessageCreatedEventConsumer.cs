using MassTransit;
using Microsoft.AspNetCore.SignalR;
using SignalRRealTimeAPI.Events;
using SignalRRealTimeAPI.Hubs;

namespace SignalRRealTimeAPI.Consumers
{
    public class MessageCreatedEventConsumer : IConsumer<MessageCreatedEvent>
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public MessageCreatedEventConsumer(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task Consume(ConsumeContext<MessageCreatedEvent> context)
        {
            var message = context.Message;

            // Mesajı ilgili gruba (chat room) gönder
            await _hubContext.Clients
                .Group(message.ChatRoomId.ToString())
                .SendAsync("ReceiveMessage", new
                {
                    message.UserId,
                    message.Content,
                    message.SentAt,
                    message.ChatRoomId,
                    message.MediaUrl
                });
        }
    }
}
