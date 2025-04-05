using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;

namespace MesajX.ChatService.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly IPostgreMessageService _postgreMessageService;
        private readonly IRedisMessageService _redisMessageService;

        public MessageService(IRedisMessageService redisMessageService, IPostgreMessageService postgreMessageService)
        {
            _redisMessageService = redisMessageService;
            _postgreMessageService = postgreMessageService;
        }

        public async Task<List<GetMessagesDto>> GetAllMessagesByRoomIdAsync(string roomId, int count)
        {
            var messages = await _postgreMessageService.GetMessagesByRoomIdAsync(roomId, count);
            return messages;
        }

        public async Task<List<GetMessagesDto>> GetRecentMessagesByRoomIdAsync(GetRecentMessagesRequestDto getRecentMessagesRequestDto)
        {
            var recentMessages = await _redisMessageService.GetRecentMessagesAsync(getRecentMessagesRequestDto);
            return recentMessages;
        }

        public async Task SetMessageAsync(SendMessageDto sendMessageDto)
        {
            await _redisMessageService.SetMessageAsync(sendMessageDto);
        }

    }
}
