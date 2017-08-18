using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer4.IdentityWebServer
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources() => new List<ApiResource>() { new ApiResource("api1", "my test api") };


        public static IEnumerable<Client> GetClients() => new List<Client>(){ new Client()
        {
            ClientId = "clientmvc",
            AllowedGrantTypes = GrantTypes.ClientCredentials,
            ClientSecrets =
            {
                new Secret("topsecret".Sha256())
            },
            AllowedScopes = {"api1"},
        },
            new Client()
            {
                ClientId = "ro.client",
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                ClientSecrets =
                {
                    new Secret("topsecret".Sha256())
                },
                AllowedScopes = {"api1"}
            },
            new Client()
            {
                ClientId = "mvc",
                ClientUri = "mvc app",
                AllowedGrantTypes = GrantTypes.Implicit,
                RedirectUris = {"http://localhost:5052/signin-oidc"},
                PostLogoutRedirectUris = {"http://localhost:5052/signout-callback-oidc"},

                AllowedScopes = new List<string>()
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                }
            }
        };

        public static List<TestUser> GetUsers() => new List<TestUser>()
        {
            new TestUser()
            {
                SubjectId = "1",
                Username = "alice",
                Password = "password",
                Claims = new []
                {
                    new Claim("name", "Alice"), 
                    new Claim("website", "https://alice.com"), 
                }
            },
            new TestUser()
            {
                SubjectId = "2",
                Username = "bob",
                Password = "password",

                Claims = new []
                {
                    new Claim("name", "Bob"),
                    new Claim("website", "https://bob.com"),
                }
            }
        };

        public static List<IdentityResource> GetIdentityResources() => new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };

    }
}