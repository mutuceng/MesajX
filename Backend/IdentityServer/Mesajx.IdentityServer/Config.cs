using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mesajx.IdentityServer;

public static class Config
{
    public static IEnumerable<ApiResource> ApiResources => new ApiResource[]
    {
        new ApiResource("ResourceChat") { Scopes = {"ChatFullPermission", "ChatReadPermission", "ChatWritePermission"}},
        new ApiResource("ResourceYARP") { Scopes = { "YARPFullPermission" } },
        new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
        {
            Scopes = { IdentityServerConstants.LocalApi.ScopeName }
        }

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
            new ApiScope("YARPFullPermission","Has full authority for YARP operations"),
            new ApiScope(IdentityServerConstants.LocalApi.ScopeName, "Local API scope") // <- burada net şekilde tanımlanmalı



        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {

            // User
            new Client
            {
                ClientId = "MesajXUserId",
                ClientName = "MesajX User",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowOfflineAccess = true,
                ClientSecrets = {new Secret("mesajxsecret".Sha256())},
                AllowedScopes =
                {
                    "ChatReadPermission", "ChatWritePermission", "YARPFullPermission", "ChatFullPermission",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.OfflineAccess,
                    IdentityServerConstants.LocalApi.ScopeName,

                },
                AccessTokenLifetime = 120,
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
                    "ChatFullPermission", "ChatReadPermission", "ChatWritePermission", "YARPFullPermission",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Email,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.LocalApi.ScopeName // <-- BURASI EKLENDİ

                },
                AccessTokenLifetime = 7200
            }
        };
}
