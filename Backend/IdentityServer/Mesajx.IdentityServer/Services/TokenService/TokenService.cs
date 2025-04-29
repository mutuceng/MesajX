using Duende.IdentityServer.Models;
using IdentityModel.Client;
using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using static System.Net.WebRequestMethods;

namespace Mesajx.IdentityServer.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly HttpClient _httpClient;
        private readonly ClientSettings _clientSettings;

        public TokenService(HttpClient httpClient, IOptions<ClientSettings> clientSettings )
        {
            _httpClient = httpClient;
            _clientSettings = clientSettings.Value;
        }

        public async Task<TokenResponseDto> GetRefreshToken(string refreshToken)
        {
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:5011",  // Örnek adres
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                }
            });

            if (discoveryEndPoint.IsError)
            {
                throw new Exception(discoveryEndPoint.Error);
            }

            // Refresh token ile yeni access token al
            var refreshTokenResponse = await _httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = discoveryEndPoint.TokenEndpoint,
                ClientId = _clientSettings.MesajXUserClient.ClientId,
                ClientSecret = _clientSettings.MesajXUserClient.ClientSecret,
                RefreshToken = refreshToken
            });

            if (refreshTokenResponse.IsError)
            {
                throw new UnauthorizedAccessException("Refresh token geçersiz");
            }

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(refreshTokenResponse.AccessToken);
            var subClaim = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(subClaim))
            {
                throw new Exception("sub claim'i token içinde bulunamadı.");
            }

            return new TokenResponseDto
            {
                AccessToken = refreshTokenResponse.AccessToken,
                RefreshToken = refreshTokenResponse.RefreshToken,
                ExpiresIn = refreshTokenResponse.ExpiresIn,
                UserId = subClaim
            };
        }

        public async Task<TokenResponseDto> GetTokenResponse(UserLoginDto userLoginDto)
        {
            var discoveryEndPoint = await _httpClient.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = "http://localhost:5011",
                Policy = new DiscoveryPolicy
                {
                    RequireHttps = false,
                }
            });

            if (discoveryEndPoint.IsError)
            {
                throw new Exception(discoveryEndPoint.Error);
            }

            var passwordTokenResponse = await _httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryEndPoint.TokenEndpoint,
                ClientId = _clientSettings.MesajXUserClient.ClientId,
                ClientSecret = _clientSettings.MesajXUserClient.ClientSecret,
                UserName = userLoginDto.Username,
                Password = userLoginDto.Password,
            });

            if (passwordTokenResponse.IsError)
                throw new Exception("Token error: " + passwordTokenResponse.Error);

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(passwordTokenResponse.AccessToken);
            var subClaim = token.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            if (string.IsNullOrEmpty(subClaim))
            {
                throw new Exception("sub claim'i token içinde bulunamadı.");
            }
            return new TokenResponseDto
            {
                AccessToken = passwordTokenResponse.AccessToken,
                RefreshToken = passwordTokenResponse.RefreshToken,
                UserId = subClaim,
                ExpiresIn = passwordTokenResponse.ExpiresIn,
            };

        }
    }
}
