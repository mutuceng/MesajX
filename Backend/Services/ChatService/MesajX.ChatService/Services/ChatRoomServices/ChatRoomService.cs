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

        public async Task CreateChatRoomAsync(CreateChatRoomDto createChatRoomDto)
        {
            await _postgreRoomChatService.CreateChatRoomAsync(createChatRoomDto);
        }

        public async Task DeleteChatRoomAsync(string chatRoomId)
        {
            var chatRoom = await _postgreRoomChatService.GetChatRoomByIdAsync(chatRoomId);
            if (chatRoom.IsGroup == false) // DM odaları silinemez
            {
                throw new InvalidOperationException("DM odaları silinemez.");
            }

            await _postgreRoomChatService.DeleteChatRoomAsync(chatRoomId);
        }

        public async Task<List<GetChatRoomByUserIdDto>> GetAllChatRoomByUserIdAsync(string userId)
        {
            var rooms = await _postgreRoomChatService.GetChatsByUserId(userId); 
            return rooms;
        }

        public async Task<GetByIdChatRoomDto> GetChatRoomByIdAsync(string chatRoomId)
        {
            var room = await _postgreRoomChatService.GetChatRoomByIdAsync(chatRoomId);
            return room; 
        }
        public async Task<bool> IsUserInRoomAsync(string userId, string chatRoomId)
        {
            var isUserInRoom = await _redisRoomChatService.IsUserInGroupChatAsync(chatRoomId, userId);
            return isUserInRoom;
        }
        public async Task UpdateChatRoomAsync(UpdateChatRoomDto updateChatRoomDto)
        {
            await _postgreRoomChatService.UpdateChatRoomAsync(updateChatRoomDto);
        }

    }
}
