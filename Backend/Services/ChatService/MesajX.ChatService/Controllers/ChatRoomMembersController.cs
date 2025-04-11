using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.Services.ChatRoomMemberServices;
using MesajX.ChatService.Services.ChatRoomServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MesajX.ChatService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatRoomMembersController : ControllerBase
    {
        private readonly IChatRoomMemberService _chatMemberService;

        public ChatRoomMembersController(IChatRoomMemberService chatMemberService)
        {
            _chatMemberService = chatMemberService;
        }

        [HttpPost("addMember")]
        public async Task<IActionResult> AddMemberToChatRoom(CreateMemberDto createMemberDto)
        {
            await _chatMemberService.AddMemberToChatRoomAsync(createMemberDto);
            return Ok(new { message = "Member added to chat room successfully" });
        }

        [HttpDelete("removeMember")]
        public async Task<IActionResult> RemoveMemberFromChatRoom(string chatRoomId, string userId)
        {
            await _chatMemberService.RemoveMemberFromChatRoomAsync(chatRoomId, userId);
            return Ok(new { message = "Member removed from chat room successfully" });
        }
    }
}
