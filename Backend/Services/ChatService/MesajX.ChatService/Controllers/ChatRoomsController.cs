using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.Services.ChatRoomServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MesajX.ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomsController(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        [HttpGet("userId")]
        public async Task<IActionResult> GetAllRoomsByUserId(string userId)
        {
             var rooms = await _chatRoomService.GetAllChatRoomByUserIdAsync(userId);
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom(CreateChatRoomDto createChatRoomDto)
        {
             await _chatRoomService.CreateChatRoomAsync(createChatRoomDto);
            return Ok(new { message = "Chat room created successfully" });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateChatRoom(UpdateChatRoomDto updateChatRoomDto)
        {
            await _chatRoomService.UpdateChatRoomAsync(updateChatRoomDto);
            return Ok(new { message = "Chat room updated successfully" });
        }

        [HttpDelete("{chatRoomId}")]
        public async Task<IActionResult> DeleteChatRoom(string chatRoomId)
        {
            await _chatRoomService.DeleteChatRoomAsync(chatRoomId);
            return Ok(new { message = "Chat room deleted successfully" });
        }


    }
}
