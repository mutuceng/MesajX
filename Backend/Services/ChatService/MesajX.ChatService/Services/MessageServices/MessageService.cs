using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.Events;
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.RabbitMQClient.Publisher;

namespace MesajX.ChatService.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly IPostgreMessageService _postgreMessageService;
        private readonly IRedisMessageService _redisMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IRabbitMQPublisher _eventPublisher;

        public MessageService(IRedisMessageService redisMessageService, IPostgreMessageService postgreMessageService, IRabbitMQPublisher eventPublisher, IChatRoomService chatRoomService)
        {
            _redisMessageService = redisMessageService;
            _postgreMessageService = postgreMessageService;
            _eventPublisher = eventPublisher;
            _chatRoomService = chatRoomService;
        }

        public async Task<List<GetMessagesDto>> GetMessagesByRoomIdAsync(string chatRoomId, int page, int pageSize)
        {
            var cachedMessages = await _redisMessageService.GetRecentMessagesAsync(new GetRecentMessagesRequestDto
            {
                ChatRoomId = chatRoomId,
                Page = page,
                PageSize = pageSize
            });

            if (cachedMessages is not null && cachedMessages.Count() == pageSize)
                return cachedMessages.OrderBy(m => m.SentAt).ToList(); 

            var remaining = pageSize - (cachedMessages?.Count() ?? 0);
            var messages = await _postgreMessageService.GetMessagesByRoomIdAsync(chatRoomId, page, remaining);

            return (cachedMessages ?? new List<GetMessagesDto>()).Concat(messages).ToList();
        }

        public async Task SetMessageAsync(SendMessageDto sendMessageDto)
        {

            bool isUserInRoom = await _chatRoomService.IsUserInRoomAsync(sendMessageDto.UserId, sendMessageDto.ChatRoomId);

            if (!isUserInRoom)
            {
                throw new UnauthorizedAccessException("User is not a member of the chat room.");
            }

            await _redisMessageService.SetMessageAsync(sendMessageDto);

            var @messageEvent = new MessageCreatedEvent
            {
                ChatRoomId = sendMessageDto.ChatRoomId,
                UserId = sendMessageDto.UserId,
                Content = sendMessageDto.Content,
                SentAt = DateTime.UtcNow, // Eğer frontend'ten gelmiyorsa
                MediaUrl = string.IsNullOrEmpty(sendMessageDto.MediaUrl) ? null : sendMessageDto.MediaUrl
            };

            await _eventPublisher.PublishMessage(@messageEvent);

            //await _signalRMessageSender.SendMessageToRoomAsync(sendMessageDto.ChatRoomId, sendMessageDto);
        }

    }
}
