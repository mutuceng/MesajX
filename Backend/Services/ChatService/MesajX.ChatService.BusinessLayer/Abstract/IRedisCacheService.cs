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
        Task<List<GetMessagesDto>> GetMessagesAsync(string groupId,int count);
        Task AddMemberToRoomAsync(string chatRoomId, int userId);
        Task<bool> IsUserInGroupChatAsync(string groupId, string userId);
    }
}
