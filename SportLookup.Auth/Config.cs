using Duende.IdentityServer.Models;

namespace SportLookup.Auth;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("main-sport-lookup-api"),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("main-sport-lookup-api-resource", "Main Api Resource")
            {
                Scopes = { "main-sport-lookup-api" }
            }
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "main-react",
                ClientSecrets = { new Secret("5E74B408-7CE7-441A-95BC-7093501FB24E".Sha256()) },
                    
                AllowedGrantTypes = GrantTypes.Implicit,

                RedirectUris = { "http://localhost:3000/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:3000/signout-oidc" },
                AllowedCorsOrigins = { "http://localhost:3000" },

                AllowOfflineAccess = true,
                AllowedScopes = { "openid", "profile", "main-sport-lookup-api" },
                AllowAccessTokensViaBrowser = true,
            },
        };
}
