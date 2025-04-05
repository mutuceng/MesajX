using MesajX.ChatService.DtoLayer.Dtos.MessageDtos;
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

        public MessagesController(ILogger<MessagesController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string chatRoomId)
        {
            int count = 100;
            var messages = await _messageService.GetAllMessagesByRoomIdAsync(chatRoomId, count);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageDto sendMessageDto)
        {
            await _messageService.SetMessageAsync(sendMessageDto);
            return Ok(new { message = "Message sent successfully" });

        }

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentMessages(string chatRoomId)
        {
            int count = 50;
            var messages = await _messageService.GetRecentMessagesByRoomIdAsync(new GetRecentMessagesRequestDto
            {
                ChatRoomId = chatRoomId,
                Count = count

            });

            return Ok(messages);
        }
    }
}
