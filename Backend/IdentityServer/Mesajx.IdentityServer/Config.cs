using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mesajx.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("ResourceChat") { Scopes = {"ChatFullPermission", "ChatReadPermission", "ChatWritePermission"}},
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
    };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("ChatFullPermission", "Has full authority for chat operations"),
            new ApiScope("ChatReadPermission", "Can read chat messages"),
            new ApiScope("ChatWritePermission", "Can write chat messages"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName)
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // Visitor
            new Client
            {
                ClientId = "MesajXVisitorId",
                ClientName = "MesajX Visitor User",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = {new Secret("mesajxsecret".Sha256())},
                AllowedScopes = {"ChatReadPermission", IdentityServerConstants.LocalApi.ScopeName },
                AllowAccessTokensViaBrowser = true
            },

            // User
            new Client
            {
                ClientId = "MesajXUserId",
                ClientName = "MesajX User",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = {new Secret("mesajxsecret".Sha256())},
                AllowedScopes =
                {
                    "ChatReadPermission", "ChatWritePermission",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                },
            },

            // Admin
            new Client
            {
                ClientId = "MesajXAdminId",
                ClientName = "MesajX Admin User",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets = {new Secret("mesajxsecret".Sha256())},
                AllowedScopes =
                {
                    "ChatFullPermission", "ChatReadPermission", "ChatWritePermission",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                AccessTokenLifetime = 7200
            }
               
                // m2m client credentials flow client
            //new Client
            //{
            //    ClientId = "m2m.client",
            //    ClientName = "Client Credentials Client",

            //    AllowedGrantTypes = GrantTypes.ClientCredentials,
            //    ClientSecrets = { new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256()) },

            //    AllowedScopes = { "scope1" }
            //},

            //// interactive client using code flow + pkce
            //new Client
            //{
            //    ClientId = "interactive",
            //    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },

            //    AllowedGrantTypes = GrantTypes.Code,

            //    RedirectUris = { "https://localhost:44300/signin-oidc" },
            //    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
            //    PostLogoutRedirectUris = { "https://localhost:44300/signout-callback-oidc" },

            //    AllowOfflineAccess = true,
            //    AllowedScopes = { "openid", "profile", "scope2" }
            //},
        };
}
