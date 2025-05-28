namespace Mesajx.IdentityServer.Dtos
{
    public class GetUsersByIdsRequest
    {
        public List<string> Ids { get; set; } = new();
    }
}
