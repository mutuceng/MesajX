using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Services.FriendshipService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mesajx.IdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpPost("add-friend")]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendshipRequestDto requestDto)
        {
            var result = await _friendshipService.SendFriendRequestAsync(requestDto);

            if (!result)
            {
                return BadRequest(new { message = "İstek zaten gönderilmiş veya gönderilemedi." });
            }

            return Ok(new { message = "Arkadaşlık isteği başarıyla gönderildi." });
        }
    }
}
