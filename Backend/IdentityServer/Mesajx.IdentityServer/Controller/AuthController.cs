using Duende.IdentityServer.Models;
using IdentityModel.Client;
using Mesajx.IdentityServer.Dtos;
using Mesajx.IdentityServer.Models;
using Mesajx.IdentityServer.Services.TokenService;
using Mesajx.IdentityServer.Services.SignInService;
using Mesajx.IdentityServer.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace Mesajx.IdentityServer.Controller
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignInService _signInService;
        private readonly ITokenService _tokenService;

        public AuthController(ISignInService signInService, ITokenService tokenService)
        {
            _signInService = signInService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> UserLogin(SignInDto signInDto)
        {
            try
            {
                var tokenResponse = await _signInService.SignIn(signInDto);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var tokenResponse = await _tokenService.GetRefreshToken(refreshTokenDto.RefreshToken);
                return Ok(tokenResponse);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
