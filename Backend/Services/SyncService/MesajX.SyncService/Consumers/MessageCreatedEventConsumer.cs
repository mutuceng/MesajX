using MassTransit;
using MesajX.RabbitMQShared.Events;
using MesajX.SyncService.Dtos;
using MesajX.SyncService.SyncServices.MessageSyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.Consumers
{
    public class MessageCreatedEventConsumer: IConsumer<MessageCreatedEvent>
    {
        private readonly IMessageSyncService _messageSyncService;
        private readonly ILogger<MessageCreatedEventConsumer> _logger;

        public MessageCreatedEventConsumer(IMessageSyncService messageSyncService, ILogger<MessageCreatedEventConsumer> logger)
        {
            _messageSyncService = messageSyncService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MessageCreatedEvent> context)
        {
            var messageEvent = context.Message;

            _logger.LogInformation("MessageCreatedEvent alındı. Mesaj ID: {MessageId}", messageEvent.MessageId);

            try
            {
                await _messageSyncService.SaveMessageToPostgreAsync(new SyncMessageDto
                {
                    MessageId = messageEvent.MessageId,
                    ChatRoomId = messageEvent.ChatRoomId,
                    UserId = messageEvent.UserId,
                    Content = messageEvent.Content,
                    SentAt = messageEvent.SentAt,
                    MediaUrl = messageEvent.MediaUrl // Eğer medya URL'si varsa burada ekleyebilirsin
                });

                _logger.LogInformation("Mesaj başarıyla PostgreSQL'e kaydedildi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mesaj PostgreSQL'e kaydedilirken hata oluştu.");
                // Hatalı event'leri dead letter kuyruğuna göndermek istersen burada işlem yapılabilir.
            }
        }
    }
}
