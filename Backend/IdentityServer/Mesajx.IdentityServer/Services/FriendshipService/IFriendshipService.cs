using Mesajx.IdentityServer.Dtos;

namespace Mesajx.IdentityServer.Services.FriendshipService
{
    public interface IFriendshipService
    {
        Task<bool> SendFriendRequestAsync(SendFriendshipRequestDto requestDto);

    }
}
