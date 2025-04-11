using MesajX.ChatService.BusinessLayer.Services.ChatRoomMemberServices.Redis;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;

namespace MesajX.ChatService.Services.ChatRoomMemberServices
{
    public class ChatRoomMemberService:IChatRoomMemberService
    {
        private readonly IRedisChatMemberService _redisChatMemberService;

        public ChatRoomMemberService(IRedisChatMemberService redisChatMemberService)
        {
            _redisChatMemberService = redisChatMemberService;
        }

        public async Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId)
        {
            var members = await _redisChatMemberService.GetMembersByRoomIdAsync(chatRoomId); // redisden hızlıca alıyoruz
            return members;
        }

        public async Task AddMemberToChatRoomAsync(CreateMemberDto createMemberDto)
        {
            await _redisChatMemberService.AddMemberToChatAsync(createMemberDto); // redisle hızlı sorgu için hashli
        }
        public async Task RemoveMemberFromChatRoomAsync(string userId, string chatRoomId)
        {
            await _redisChatMemberService.RemoveMemberFromChatAsync(chatRoomId, userId); // hashten silme
        }

    }
}
