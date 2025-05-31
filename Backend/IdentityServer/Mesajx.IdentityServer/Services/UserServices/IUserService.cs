using Mesajx.IdentityServer.Dtos;

namespace Mesajx.IdentityServer.Services.UserServices
{
    public interface IUserService
    {
        Task<List<UserInfoDto>> GetUsersByIdsAsync(List<string> userIds);

        Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateDto);
    }
}
