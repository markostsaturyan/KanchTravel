using IdentityServer4.Models;
using System.Collections.Generic;

namespace Authentication
{
    /// <summary>
    /// Configure secured resources and authenticated clients
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Get secured resources
        /// </summary>
        /// <returns> Api resources </returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("campingTrip", "CampingTrip"),
                new ApiResource("userManagement","UserManagement")
            };
        }

        /// <summary>
        /// Get clients
        /// </summary>
        /// <returns> List of clients </returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "kanchDesktopApp",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes = { "campingTrip", "userManagement", "offline_access"},
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 15780000
                },

                new Client
                {
                    ClientId="campingTrip",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes ={"userManagement"}
                },

                new Client
                {
                    ClientId="userManagement",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    AllowedScopes ={ "campingTrip" },
                }
            };
        }

        /// <summary>
        /// Gets Identity resources
        /// </summary>
        /// <returns> List of Identity resources </returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResource {
                Name = "Role",
                UserClaims = new List<string> {"Admin","User","Driver","Guide","Photographer"}}
            };
        }
    }
}
