using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mesajx.IdentityServer.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public LoginsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserLogin(UserLoginDto userLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userLoginDto.Username);
            if (user == null)
            {
                return BadRequest("Kullanici bulunamadi.");
            }
            var result = await _signInManager.PasswordSignInAsync(user, userLoginDto.Password, false, false);
            // isPersistent: false, lockoutOnFailure: false
            if (result.Succeeded)
            {
                return Ok("Kullanici basariyla giris yapti.");
            }
            return BadRequest("Kullanici adi veya sifre hatali.");
        }
    }
}
