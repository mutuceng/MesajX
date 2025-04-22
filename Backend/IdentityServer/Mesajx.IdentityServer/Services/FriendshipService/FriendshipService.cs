using Mesajx.IdentityServer.Data;
using Mesajx.IdentityServer.Dtos;
using Microsoft.EntityFrameworkCore;
using static Mesajx.IdentityServer.Models.FrendshipRequest;

namespace Mesajx.IdentityServer.Services.FriendshipService
{
    public class FriendshipService : IFriendshipService
    {
        private readonly ApplicationDbContext _context;

        public FriendshipService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> SendFriendRequestAsync(SendFriendshipRequestDto requestDto)
        {
            var existingRequest = await _context.FriendshipRequests
                .FirstOrDefaultAsync(fr =>
                    fr.SenderUserId == requestDto.SenderUserId &&
                    fr.ReceiverUserId == requestDto.ReceiverUserId);

            if (existingRequest != null)
            {
                // Daha önce gönderilmiş
                return false;
            }

            // Zaten arkadaşlar mı kontrolü yapılabilir (eğer arkadaşlık ilişkisini ayrı tutuyorsan)

            var newRequest = new FriendshipRequest
            {
                SenderUserId = requestDto.SenderUserId,
                ReceiverUserId = requestDto.ReceiverUserId,
                CreatedAt = DateTime.UtcNow
            };

            _context.FriendshipRequests.Add(newRequest);
            var result = await _context.SaveChangesAsync();

            return result > 0;

        }
    }
}
