using IdentityServer4.Models;
using System.Collections.Generic;

namespace Authentication
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("compingTrip", "CompingTrip")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                // resource owner password grant client
                new Client
                {
                    ClientId = "CompingTrip",

                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "compingTrip" }
                }


            };

        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource> {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Email(),
            new IdentityResource {
                Name = "role",
                UserClaims = new List<string> {"role"}
            }
        };

        }
    }
}
