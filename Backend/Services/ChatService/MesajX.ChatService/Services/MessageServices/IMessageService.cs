using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;

namespace MesajX.ChatService.Services.MessageServices
{
    public interface IMessageService
    {
        Task<List<GetMessagesDto>> GetMessagesByRoomIdAsync(string chatRoomId, int page, int pageSize);
        Task SetMessageAsync(SendMessageDto sendMessageDto);
    }
}
