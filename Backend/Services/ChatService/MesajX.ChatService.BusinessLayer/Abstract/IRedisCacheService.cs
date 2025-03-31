using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Abstract
{
    public interface IRedisCacheService
    {
        Task SetMessageAsync(SendMessageDto sendMessageDto);
        Task<List<GetMessagesDto>> GetMessagesAsync(int count);
        Task<bool> IsUserInGroupChatAsync(string groupId, string userId);
    }
}
