using MassTransit;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomMemberServices.Redis;
using MesajX.ChatService.BusinessLayer.Services.ChatRoomServices.Redis;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.RabbitMQShared.Events;

namespace MesajX.ChatService.Services.ChatRoomMemberServices
{
    public class ChatRoomMemberService:IChatRoomMemberService
    {
        private readonly IRedisChatMemberService _redisChatMemberService;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<ChatRoomMemberService> _logger;

        public ChatRoomMemberService(IRedisChatMemberService redisChatMemberService, IPublishEndpoint publishEndpoint, ILogger<ChatRoomMemberService> logger)
        {
            _redisChatMemberService = redisChatMemberService;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task<List<GetMembersByRoomIdDto>> GetMembersByRoomIdAsync(string chatRoomId)
        {
            var members = await _redisChatMemberService.GetMembersByRoomIdAsync(chatRoomId); // redisden hızlıca alıyoruz
            return members;
        }

        public async Task AddMemberToChatRoomAsync(CreateMemberDto createMemberDto)
        {

            try
            {

                await _redisChatMemberService.AddMemberToChatAsync(createMemberDto); // redisle hızlı sorgu için hashli

                await _publishEndpoint.Publish<MemberAddedEvent>(new
                {
                    ChatRoomId = createMemberDto.ChatRoomId,
                    UserId = createMemberDto.UserId,
                    Role = createMemberDto.Role,
                });

                // Başarılı olduğunu logla
                _logger.LogInformation($"Member event published for ChatRoomId: {createMemberDto.ChatRoomId}");



            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to publish message event for ChatRoomId: {createMemberDto.ChatRoomId}");
                // Hata durumunda ne yapacağınıza karar verin (yeniden deneme, hata fırlatma, vb.)
                throw;
            }
        }
        public async Task RemoveMemberFromChatRoomAsync(string userId, string chatRoomId)
        {
            await _redisChatMemberService.RemoveMemberFromChatAsync(chatRoomId, userId); // hashten silme
        }

    }
}
