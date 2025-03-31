using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mesajx.IdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistersController(UserManager<ApplicationUser> userManager)
        {
           _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister(UserRegisterDto userRegisterDto)
        {
            var user = new ApplicationUser
            {
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                ProfileImageUrl = userRegisterDto.ProfileImageUrl,
                BirthDate = userRegisterDto.BirthDate,
                PhoneNumber = userRegisterDto.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (result.Succeeded)
            {
                return Ok("Kullanici basariyla kaydoldu.");
            }
            return BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}
