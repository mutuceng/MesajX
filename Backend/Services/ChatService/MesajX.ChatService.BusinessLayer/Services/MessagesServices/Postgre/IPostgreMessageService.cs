using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre
{
    public interface IPostgreMessageService
    {
        Task<List<GetMessagesDto>> GetMessagesByRoomIdAsync(string roomId, int count);
        Task SaveMessagesAsync(List<SendMessageDto> sendMessageDtos, CancellationToken stoppingToken);

    }
}
