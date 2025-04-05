namespace Mesajx.IdentityServer.Models
{
    public class FrendshipRequest
    {
        public enum FriendshipRequestStatus
        {
            Pending = 0, // Beklemede
            Accepted = 1, // Kabul edildi
        }

        public class FriendshipRequest
        {
            public string FriendshipRequestId { get; set; } = Guid.NewGuid().ToString();
            public int SenderUserId { get; set; } // İsteği gönderen kullanıcı
            public int ReceiverUserId { get; set; } // İsteği alan kullanıcı
            public FriendshipRequestStatus Status { get; set; } = FriendshipRequestStatus.Pending;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        }

    }
}
