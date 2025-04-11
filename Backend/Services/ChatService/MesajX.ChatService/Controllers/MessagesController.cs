using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
using MesajX.ChatService.Services.ChatRoomServices;
using MesajX.ChatService.Services.MessageServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MesajX.ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly ILogger<MessagesController> _logger;
        private readonly IMessageService _messageService;
        private readonly IChatRoomService _chatRoomService;

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService, IChatRoomService chatRoomService)
        {
            _logger = logger;
            _messageService = messageService;
            _chatRoomService = chatRoomService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string chatRoomId, int page)
        {
            int pageSize = 250;
            var messages = await _messageService.GetMessagesByRoomIdAsync(chatRoomId, page, pageSize);
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
