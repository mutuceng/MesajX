using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;

namespace MesajX.ChatService.Services.MessageServices
{
    public interface IMessageService
    {
        Task<List<GetMessagesDto>> GetRecentMessagesByRoomIdAsync(GetRecentMessagesRequestDto getRecentMessagesRequestDto);
        Task<List<GetMessagesDto>> GetAllMessagesByRoomIdAsync(string chatRoomId, int count);
        Task SetMessageAsync(SendMessageDto sendMessageDto);
    }
}
