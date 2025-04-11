using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;

namespace MesajX.ChatService.Services.ChatRoomMemberServices
{
    public interface IChatRoomMemberService
    {
        Task AddMemberToChatRoomAsync(CreateMemberDto createMemberDto);
        Task RemoveMemberFromChatRoomAsync(string chatRoomId, string userId);

        Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId);
    }
}
