using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;

namespace MesajX.ChatService.Services.ChatRoomServices
{
    public class ChatRoomService : IChatRoomService
    {
        private readonly IPostgresRoomChatService _postgreRoomChatService;
        private readonly IRedisRoomChatService _redisRoomChatService;

        public ChatRoomService(IPostgresRoomChatService postgreRoomChatService, IRedisRoomChatService redisRoomChatService)
        {
            _postgreRoomChatService = postgreRoomChatService;
            _redisRoomChatService = redisRoomChatService;
        }

        public async Task AddMemberToChatRoomAsync(CreateMemberDto createMemberDto)
        {
            await _postgreRoomChatService.AddMemberToChatAsync(createMemberDto); // asıl kayıt 
            await _redisRoomChatService.AddMemberToChatAsync(createMemberDto, createMemberDto.ChatRoomId ); // redisle hızlı sorgu için hashli

        }

        public async Task CreateChatRoomAsync(CreateChatRoomDto createChatRoomDto)
        {
            await _postgreRoomChatService.CreateChatRoomAsync(createChatRoomDto); 
        }

        public async Task DeleteChatRoomAsync(string chatRoomId)
        {
            await _postgreRoomChatService.DeleteChatRoomAsync(chatRoomId);
        }

        public async Task<List<GetChatRoomByUserIdDto>> GetAllChatRoomByUserIdAsync(string userId)
        {
            var rooms = await _postgreRoomChatService.GetChatsByUserId(userId); 
            return rooms;
        }

        public async Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId)
        {
            var members = await _redisRoomChatService.GetMembersByRoomIdAsync(chatRoomId); // redisden hızlıca alıyoruz
            return members;
        }

        public Task<bool> IsUserInRoomAsync(string userId, string chatRoomId)
        {
            var isUserInRoom = _redisRoomChatService.IsUserInGroupChatAsync(userId, chatRoomId);
            return isUserInRoom;
        }

        public async Task RemoveMemberFromChatRoomAsync(string chatRoomId, string userId)
        {
            await _postgreRoomChatService.RemoveMemberFromChatAsync(chatRoomId, userId); 
            await _redisRoomChatService.RemoveMemberFromChatAsync(chatRoomId, userId); // hashten silme
        }

        public async Task UpdateChatRoomAsync(UpdateChatRoomDto updateChatRoomDto)
        {
            await _postgreRoomChatService.UpdateChatRoomAsync(updateChatRoomDto); 
        }
    }
}
