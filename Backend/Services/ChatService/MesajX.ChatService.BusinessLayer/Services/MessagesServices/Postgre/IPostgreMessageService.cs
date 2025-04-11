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
        Task<List<GetMessagesDto>> GetMessagesByRoomIdAsync(string chatRoomId, int page, int remaining);
        Task SaveMessagesAsync(List<SendMessageDto> sendMessageDtos, CancellationToken stoppingToken);

    }
}
