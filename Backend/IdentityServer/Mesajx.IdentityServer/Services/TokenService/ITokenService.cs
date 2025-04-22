using Mesajx.IdentityServer.Dtos;

namespace Mesajx.IdentityServer.Services.TokenService
{
    public interface ITokenService
    {
        Task<TokenResponseDto> GetRefreshToken(string refreshToken);
        Task<TokenResponseDto> GetTokenResponse(UserLoginDto userLoginDto);

    }
}
