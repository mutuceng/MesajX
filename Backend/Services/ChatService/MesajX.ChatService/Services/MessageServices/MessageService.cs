using MassTransit;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Postgre;
using MesajX.ChatService.BusinessLayer.Services.MessagesServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.RabbitMQShared.Events;

namespace MesajX.ChatService.Services.MessageServices
{
    public class MessageService : IMessageService
    {
        private readonly IPostgreMessageService _postgreMessageService;
        private readonly IRedisMessageService _redisMessageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<MessageService> _logger;

        public MessageService(IRedisMessageService redisMessageService, IPostgreMessageService postgreMessageService, IPublishEndpoint publishEndpoint, IChatRoomService chatRoomService, ILogger<MessageService> logger)
        {
            _redisMessageService = redisMessageService;
            _postgreMessageService = postgreMessageService;
            _publishEndpoint = publishEndpoint;
            _chatRoomService = chatRoomService;
            _logger = logger;
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

            try
            {
                await _publishEndpoint.Publish<MessageCreatedEvent>(new
                {
                    MessageId = sendMessageDto.MessageId,
                    ChatRoomId = sendMessageDto.ChatRoomId,
                    UserId = sendMessageDto.UserId,
                    Content = sendMessageDto.Content,
                    SentAt = DateTime.UtcNow,
                    MediaUrl = string.IsNullOrEmpty(sendMessageDto.MediaUrl) ? null : sendMessageDto.MediaUrl
                });

                // Başarılı olduğunu logla
                _logger.LogInformation($"Message event published for ChatRoomId: {sendMessageDto.ChatRoomId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish message event for ChatRoomId: {sendMessageDto.ChatRoomId}");
                // Hata durumunda ne yapacağınıza karar verin (yeniden deneme, hata fırlatma, vb.)
                throw;
            }

            //await _signalRMessageSender.SendMessageToRoomAsync(sendMessageDto.ChatRoomId, sendMessageDto);
        }

    }
}
