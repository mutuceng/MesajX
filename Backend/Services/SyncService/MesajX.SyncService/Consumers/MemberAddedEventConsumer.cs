using MassTransit;
using MesajX.RabbitMQShared.Events;
using MesajX.SyncService.Dtos;
using MesajX.SyncService.SyncServices.MemberSyncService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.Consumers
{
    public class MemberAddedEventConsumer : IConsumer<MemberAddedEvent>
    {
        private readonly IMemberSyncService _memberSyncService;
        private readonly ILogger<MemberAddedEventConsumer> _logger;

        public MemberAddedEventConsumer(IMemberSyncService memberSyncService, ILogger<MemberAddedEventConsumer> logger)
        {
            _memberSyncService = memberSyncService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MemberAddedEvent> context)
        {
            var memberEvent = context.Message;

            _logger.LogInformation("MessageCreatedEvent alındı. Mesaj ID: {MessageId}", memberEvent.ChatRoomId);

            try
            {
                await _memberSyncService.AddMemberToPostgreAsync(new SyncMemberDto
                {
                    UserId = memberEvent.UserId,
                    ChatRoomId = memberEvent.ChatRoomId,
                    Role = memberEvent.Role.HasValue ? (MesajX.SyncService.Dtos.Role?)Enum.Parse(typeof(MesajX.SyncService.Dtos.Role), memberEvent.Role.Value.ToString()) : null
                });

                _logger.LogInformation("Üye başarıyla PostgreSQL'e kaydedildi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Üye PostgreSQL'e kaydedilirken hata oluştu.");
                // Hatalı event'leri dead letter kuyruğuna göndermek istersen burada işlem yapılabilir.  
            }
        }
    }
}
