using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreTemp.IdentityServer
{
    public static class Configur
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()

            };



        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets  = { new Secret("client_secret_mvc".ToSha256()) },

                    AllowedGrantTypes = GrantTypes.Code,

                   RedirectUris = {"http://localhost:49241/signin-oidc"},
                   PostLogoutRedirectUris = { "http://localhost:49241/Home/Index"},
                    AllowedScopes = {
                        "ApiOne",
                        "ApiTwo",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                    },
                    RequireConsent = false,
                }
            };
    }
}
