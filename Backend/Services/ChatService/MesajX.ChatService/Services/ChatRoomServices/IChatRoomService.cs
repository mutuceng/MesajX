using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;

namespace MesajX.ChatService.Services.ChatRoomServices
{
    public interface IChatRoomService
    {
        Task<List<GetChatRoomByUserIdDto>> GetAllChatRoomByUserIdAsync(string userId);
        Task<bool> IsUserInRoomAsync(string userId, string chatRoomId);
        Task CreateChatRoomAsync(CreateChatRoomDto createChatRoomDto);
        Task UpdateChatRoomAsync(UpdateChatRoomDto updateChatRoomDto);
        Task DeleteChatRoomAsync(string chatRoomId);
        Task<GetByIdChatRoomDto> GetChatRoomByIdAsync(string chatRoomId);
    }
}
