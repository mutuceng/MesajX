using Mesajx.IdentityServer.Data;
using Mesajx.IdentityServer.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mesajx.IdentityServer.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserInfoDto>> GetUsersByIdsAsync(List<string> userIds)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new UserInfoDto
                {
                    Id = u.Id,
                    Username = u.UserName,
                    ProfileImageUrl = u.ProfileImageUrl
                })
                .ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(string userId, UpdateUserDto updateDto)
        {
            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return false;

            // Sadece username ve profileImageUrl güncelleniyor
            user.UserName = updateDto.Username;
            user.ProfileImageUrl = updateDto.ProfileImageUrl;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }


    }
}
