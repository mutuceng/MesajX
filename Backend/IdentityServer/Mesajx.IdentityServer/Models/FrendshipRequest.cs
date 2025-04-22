namespace Mesajx.IdentityServer.Models
{
    public class FrendshipRequest
    {
        public enum FriendshipRequestStatus
        {
            Pending = 0, 
            Accepted = 1, 
        }

        public class FriendshipRequest
        {
            public string FriendshipRequestId { get; set; } = Guid.NewGuid().ToString();
            public string SenderUserId { get; set; } 
            public ApplicationUser SenderUser { get; set; }
            public string ReceiverUserId { get; set; } 
            public ApplicationUser ReceiverUser { get; set; }

            public FriendshipRequestStatus Status { get; set; } = FriendshipRequestStatus.Pending;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }
    }
}
