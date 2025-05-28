using Mesajx.IdentityServer.Models;
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

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
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
                Email = user.Email,
                UserName = user.UserName
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

        [HttpGet("getchatusers")]
        public async Task<IActionResult> GetChatUsers()
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
                Email = user.Email,
                UserName = user.UserName
            });
        }
    }
}
