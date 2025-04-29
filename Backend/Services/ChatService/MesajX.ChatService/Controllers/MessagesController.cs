using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.Hubs;
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.ChatService.Services.MessageServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace MesajX.ChatService.Controllers
{
    [Authorize]
    [Route("api/chat/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageService _messageService;
        private readonly IChatRoomService _chatRoomService;
        private readonly IHubContext<ChatHub> _hubContext;

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService, IChatRoomService chatRoomService, IHubContext<ChatHub> hubContext)
        {
            _logger = logger;
            _messageService = messageService;
            _chatRoomService = chatRoomService;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string chatRoomId, int page)
        {
            int pageSize = 250;
            var messages = await _messageService.GetMessagesByRoomIdAsync(chatRoomId, page, pageSize);

            if (messages == null || !messages.Any())
            {
                return Ok(new { message = "No messages have been sent yet." });
            }

            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto sendMessageDto)
        {
            try
            {
                bool isUserInRoom = await _chatRoomService.IsUserInRoomAsync(sendMessageDto.UserId, sendMessageDto.ChatRoomId);

                if (!isUserInRoom)
                {
                    return BadRequest("User is not in the chat room");
                }
                else
                {
                    await _messageService.SetMessageAsync(sendMessageDto);

                    await _hubContext.Clients.Group(sendMessageDto.ChatRoomId).SendAsync(
                                "ReceiveMessage",
                                new
                                {
                                    sendMessageDto.MessageId,
                                    sendMessageDto.UserId,
                                    sendMessageDto.ChatRoomId,
                                    sendMessageDto.Content,
                                    sendMessageDto.MediaUrl,
                                    sendMessageDto.SentAt,
                                });
                    return Ok(new { message = "Message sent successfully" });


                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message for user {UserId} in chat room {ChatRoomId}", sendMessageDto?.UserId, sendMessageDto?.ChatRoomId);
                return StatusCode(500, "An error occurred while sending the message.");
            }


        }
    }
}
