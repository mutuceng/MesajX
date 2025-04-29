namespace Mesajx.IdentityServer.Settings
{
    public class ClientSettings
    {  
        public Client MesajXUserClient { get; set; }
        public Client MesajXAdminClient { get; set; }
        

        public class Client
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
        }
    }
}
