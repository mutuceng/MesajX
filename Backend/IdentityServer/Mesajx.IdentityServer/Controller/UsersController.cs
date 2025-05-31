using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Mesajx.IdentityServer.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using static Duende.IdentityServer.IdentityServerConstants;

namespace Mesajx.IdentityServer.Controller
{
    [Authorize(LocalApi.PolicyName)]
    [Route("api/user/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserService _userService;

        public UsersController(UserManager<ApplicationUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet("GetUserInfo")]
        public async Task<IActionResult> GetUserInfo()
        {
            var userClaim = User.Claims.FirstOrDefault(x => 
                            x.Type == JwtRegisteredClaimNames.Sub || 
                            x.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userClaim == null)
            {
                return Unauthorized("Kullanıcı kimlik bilgisi bulunamadı.");
            }

            var user = await _userManager.FindByIdAsync(userClaim.Value);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(new
            {
                Id = user.Id,
                Username = user.UserName,
                ProfileImageUrl = user.ProfileImageUrl
            });
        }

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetUserIdByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { title = "Kullanıcı adı boş olamaz" });
            }

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return NotFound(new { title = "Kullanıcı bulunamadı" });
            }

            return Ok(new { userId = user.Id });
        }

        [HttpPost("getchatusers")]
        public async Task<ActionResult<List<UserInfoDto>>> GetChatUsers([FromBody] GetUsersByIdsRequest request)
        {
            if (request.userIds == null || !request.userIds.Any())
                return BadRequest("ID listesi boş olamaz.");

            var users = await _userService.GetUsersByIdsAsync(request.userIds);

            var result = users.Select(u => new UserInfoDto
            {
                Id = u.Id,
                Username = u.Username,
                ProfileImageUrl = u.ProfileImageUrl
            }).ToList();

            return Ok(result);
        }

        [HttpPut("updateprofile")]
        public async Task<IActionResult> UpdateUser([FromQuery] string userId, [FromBody] UpdateUserDto updateDto)
        {
            if (string.IsNullOrWhiteSpace(userId) || updateDto == null)
                return BadRequest("Geçersiz kullanıcı bilgisi.");

            var result = await _userService.UpdateUserAsync(userId, updateDto);

            if (!result)
                return NotFound("Kullanıcı bulunamadı.");

            return NoContent(); // 204
        }
    }
}
