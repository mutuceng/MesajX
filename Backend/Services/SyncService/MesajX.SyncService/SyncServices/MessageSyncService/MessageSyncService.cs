using MesajX.ChatService.DataAccessLayer.Concrete;
using MesajX.ChatService.EntityLayer.Entities;
using MesajX.SyncService.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.SyncService.SyncServices.MessageSyncService
{
    public class MessageSyncService : IMessageSyncService
    {
        private readonly ChatContext _chatContext;
        private readonly ILogger<MessageSyncService> _logger;

        public MessageSyncService(ChatContext chatContext, ILogger<MessageSyncService> logger)
        {
            _chatContext = chatContext;
            _logger = logger;
        }

        public async Task SaveMessageToPostgreAsync(SyncMessageDto messageDto)
        {
            var message = new Message
            {
                MessageId = messageDto.MessageId,
                Content = messageDto.Content,
                UserId = messageDto.UserId,
                SentAt = messageDto.SentAt,
                ChatRoomId = messageDto.ChatRoomId,
                MediaUrl = messageDto.MediaUrl,
            };

            try
            {
                await _chatContext.Set<Message>().AddAsync(message);
                await _chatContext.SaveChangesAsync();
                _logger.LogInformation("Message saved successfully to Postgre: {MessageId}", message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving message: {MessageId}", message.MessageId);
                throw;
            }
        }
    }
}
