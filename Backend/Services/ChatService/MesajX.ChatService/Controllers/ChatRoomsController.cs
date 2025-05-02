using MesajX.ChatService.DtoLayer.Dtos.ChatRoomDtos;
using MesajX.ChatService.DtoLayer.Dtos.ChatRoomMemberDtos;
using MesajX.ChatService.Services.ChatRoomServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MesajX.ChatService.Controllers
{
    [Authorize]
    [Route("api/chat/[controller]")]
    [ApiController]
    public class ChatRoomsController : ControllerBase
    {
        private readonly IChatRoomService _chatRoomService;

        public ChatRoomsController(IChatRoomService chatRoomService)
        {
            _chatRoomService = chatRoomService;
        }

        // [HttpGet("userId")] bu sekil olursa eğer ? ile query
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllRoomsByUserId(string userId)
        {
             var rooms = await _chatRoomService.GetAllChatRoomByUserIdAsync(userId);
            return Ok(rooms);
        }

        [HttpGet("room/{chatRoomId}")]
        public async Task<IActionResult> GetChatRoomById(string chatRoomId)
        {
            var rooms = await _chatRoomService.GetChatRoomByIdAsync(chatRoomId);
            return Ok(rooms);
        }

        [HttpPost]
        public async Task<IActionResult> CreateChatRoom([FromForm] CreateRoomDto createRoom, [FromForm] IFormFile groupImage)
        {

            try
            {
                var chatRoom = new CreateChatRoomDto
                {
                    ChatRoomId = createRoom.ChatRoomId,
                    Name = createRoom.Name,
                    IsGroup = createRoom.IsGroup,
                    CreatedAt = createRoom.CreatedAt
                };


                string? photoPath = null;
                if (groupImage != null)
                {
                    // Dosya adını oluştur (örneğin, GUID + orijinal dosya uzantısı)
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(groupImage.FileName)}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                    // Klasörün var olduğundan emin ol
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Dosyayı kaydet
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await groupImage.CopyToAsync(stream);
                    }

                    // Dosya yolunu sakla (örneğin, veritabanına kaydetmek için)
                    photoPath = $"/uploads/{fileName}";
                }
                else
                {
                    photoPath = "/uploads/default.png"; 
                }

                chatRoom.Photo = photoPath;

                await _chatRoomService.CreateChatRoomAsync(chatRoom);

                return Ok(new
                {
                    Message = "Sohbet odası oluşturuldu",
                    ChatRoomId = chatRoom.ChatRoomId,
                    PhotoPath = photoPath
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Bir hata oluştu", Details = ex.Message });
            }
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
