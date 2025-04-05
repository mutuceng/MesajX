using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis;
using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.EntityLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.BackgroundServices
{
    public class MessageSyncService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MessageSyncService> _logger;

        public MessageSyncService(IServiceProvider serviceProvider, ILogger<MessageSyncService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Message sync service starting");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SyncMessages(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while syncing messages");
                }


                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
            _logger.LogInformation("Message sync service stopping");

        }

        private async Task SyncMessages(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var redisMessageService = scope.ServiceProvider.GetRequiredService<IRedisMessageService>();
            var postgreMessageService = scope.ServiceProvider.GetRequiredService<IPostgreMessageService>();

            var db = scope.ServiceProvider.GetRequiredService<ChatContext>();

            // son 1 saatte mesaj gönderilmiş odalar
            var chatRooms = await db.Set<ChatRoom>()
                            .Where(cr => cr.Messages.Any(m => m.SentAt >= DateTime.UtcNow.AddHours(-1)))
                            .ToListAsync(stoppingToken);

            foreach (var room in chatRooms)
            {
                try
                {
                    _logger.LogInformation($"Syncing messages for chat room: {room.ChatRoomId}");

                    var latestMessageTimestamp = await db.Set<Message>()
                        .Where(m => m.ChatRoomId == room.ChatRoomId)
                        .OrderByDescending(m => m.SentAt)
                        .Select(m => m.SentAt)
                        .FirstOrDefaultAsync(stoppingToken);

                    DateTime? since = latestMessageTimestamp != default ? latestMessageTimestamp : null;


                    var request = new GetRecentMessagesRequestDto
                    {
                        ChatRoomId = room.ChatRoomId,
                        Count = 100,
                        LastMessageDate = since
                    };

                    var redisMessages = await redisMessageService.GetRecentMessagesAsync(request);
                    if (redisMessages.Any())
                    {
                        _logger.LogInformation("Found {Count} new messages for room {RoomId}", redisMessages.Count, room.ChatRoomId);

                        var messageIds = redisMessages.Select(m => m.MessageId).ToList();
                        var existingMessageIds = await db.Set<Message>()
                                                    .Where(m => m.ChatRoomId == room.ChatRoomId && messageIds.Contains(m.MessageId))
                                                    .Select(m => m.MessageId)
                                                    .ToListAsync(stoppingToken);


                        var newMessages = redisMessages
                                        .Where(msg => !existingMessageIds.Contains(msg.MessageId))
                                        .Select(redisMsg => new SendMessageDto
                                        {
                                            ChatRoomId = room.ChatRoomId,
                                            UserId = redisMsg.UserId,
                                            Content = redisMsg.Content,
                                            SentAt = redisMsg.SentAt,
                                            MessageId = redisMsg.MessageId
                                        }).ToList();

                        //var newMessages = redisMessages
                        //                .Where(msg => !existingMessageIds.Contains(msg.MessageId))
                        //                .Select(redisMsg => _mapper.Map<SendMessageDto>(redisMsg))
                        //                .ToList();

                        if (newMessages.Any())
                        {
                            // PostgreSQL'e kaydetme
                            await postgreMessageService.SaveMessagesAsync(newMessages, stoppingToken);

                            _logger.LogInformation("Successfully saved {Count} new messages to PostgreSQL for room {RoomId}",
                                newMessages.Count, room.ChatRoomId);
                        }
                        else
                        {
                            _logger.LogInformation("All messages already exist in PostgreSQL for room {RoomId}", room.ChatRoomId);
                        }


                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing messages for room {RoomId}", room.ChatRoomId);
                    // Continue with next room despite error
                }
            }
        }
    }
}
