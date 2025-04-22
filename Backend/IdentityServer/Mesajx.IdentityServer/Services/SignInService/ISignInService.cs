using Mesajx.IdentityServer.Dtos;

namespace Mesajx.IdentityServer.Services.SignInService
{
    public interface ISignInService
    {
        Task<TokenResponseDto> SignIn(SignInDto signInDto);
    }
}
