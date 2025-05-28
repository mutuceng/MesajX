using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mesajx.IdentityServer.Controller
{
    [Route("api/user/[controller]")]
    [ApiController]
    public class RegistersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistersController(UserManager<ApplicationUser> userManager)
        {
           _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UserRegister([FromForm] UserRegisterDto userRegisterDto, [FromForm] IFormFile profileImage)
        {
            var user = new ApplicationUser
            {
                UserName = userRegisterDto.Username,
                Email = userRegisterDto.Email,
                BirthDate = DateTime.SpecifyKind(userRegisterDto.BirthDate, DateTimeKind.Utc),
                PhoneNumber = userRegisterDto.PhoneNumber
            };

            string profileImagePath = "";

            if(profileImage != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(profileImage.FileName)}";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/profileImages", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await profileImage.CopyToAsync(stream);
                }

                profileImagePath = $"/uploads/profileImages/{fileName}";
            }
            else
            {
                profileImagePath = "/uploads/profileImages/default.jpg";
            }

            user.ProfileImageUrl = profileImagePath;

            var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
            if (result.Succeeded)
            {
                return Ok("Kullanici basariyla kaydoldu.");
            }
            return BadRequest(result.Errors.Select(e => e.Description));
        }
    }
}
