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

        public MessageSyncService(ChatContext chatContext)
        {
            _chatContext = chatContext;
        }

        public async Task SaveMessageToPostgreAsync(SyncMessageDto messageDto)
        {
            var message = new Message
            {
                Content = messageDto.Content,
                UserId = messageDto.UserId,
                SentAt = messageDto.SentAt,  // Mesajın oluşturulma tarihi
                ChatRoomId = messageDto.ChatRoomId,
                MediaUrl = messageDto.MediaUrl,
            };

            // Veritabanına ekleme işlemi
            await _chatContext.Set<Message>().AddAsync(message);
            await _chatContext.SaveChangesAsync(); // Değişiklikleri kayde
        }
    }
}
