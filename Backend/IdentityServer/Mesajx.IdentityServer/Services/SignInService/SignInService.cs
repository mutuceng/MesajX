using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Mesajx.IdentityServer.Services.TokenService;
using Microsoft.AspNetCore.Identity;

namespace Mesajx.IdentityServer.Services.SignInService
{
    public class SignInService : ISignInService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignInService(ITokenService tokenService, UserManager<ApplicationUser> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<TokenResponseDto> SignIn(SignInDto signInDto)
        {
            // Kullanıcıyı veritabanından bul  
            var user = await _userManager.FindByNameAsync(signInDto.UserName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Kullanıcı bulunamadı");
            }

            // Şifreyi doğrula  
            bool isPasswordValid = await _userManager.CheckPasswordAsync(user, signInDto.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Şifre hatalı");
            }

            // Token üret  
            var tokenResponse = await _tokenService.GetTokenResponse(new UserLoginDto
            {
                Username = signInDto.UserName,
                Password = signInDto.Password // Veya kullanıcının Id'si vs.  
            });

            return tokenResponse;
        }
    }
}
