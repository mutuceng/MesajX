namespace Mesajx.IdentityServer.Dtos
{
    public class GetUsersByIdsRequest
    {
        public List<string> userIds { get; set; } = new();
    }
}
