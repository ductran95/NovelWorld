using System.Collections.Generic;
using IdentityServer4.Models;

namespace NovelWorld.Identity.Data.Configurations
{
    public class IdentityServerConfig
    {
        public string CertificatePassword { get; set; }
        // ApiResources define the apis in your system
        public IEnumerable<ApiScope> ApiScopes { get; set; }
        public IEnumerable<ApiResource> ApiResources { get; set; }

        // Identity resources are data like user ID, name, or email address of a user
        // see: http://docs.identityserver.io/en/release/configuration/resources.html
        public IEnumerable<IdentityResource> IdentityResources { get; set; }

        // client want to access resources (aka scopes)
        public IEnumerable<Client> Clients { get; set; }

        // Open ID Connect
        public IEnumerable<CustomOpenIdConnectOptions> OpenIdProviders { get; set; }
    }
    
    public class CustomOpenIdConnectOptions
    {
        public string AuthenticationScheme { get; set; }
        public string DisplayName { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string ClaimsIssuer { get; set; }
        public string MetadataAddress { get; set; }
        public bool GetClaimsFromUserInfoEndpoint { get; set; }
        public string SignInScheme { get; set; }
        public string ResponseType { get; set; }
    }
}