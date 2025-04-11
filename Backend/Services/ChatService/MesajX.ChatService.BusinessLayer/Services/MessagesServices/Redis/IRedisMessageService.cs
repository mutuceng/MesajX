using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis
{
    public interface IRedisMessageService
    {
        Task SetMessageAsync(SendMessageDto sendMessageDto);
        Task<List<GetMessagesDto>> GetRecentMessagesAsync(GetRecentMessagesRequestDto getRecentMessagesRequestDto);
        
    }
}
