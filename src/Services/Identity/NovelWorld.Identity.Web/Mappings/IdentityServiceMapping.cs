using System.Net.Http;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Identity.Data.Configurations;

namespace NovelWorld.Identity.Web.Mappings
{
    public static class IdentityServiceMapping
    {
        public static IServiceCollection AddAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<IdentityAppSettings>(config);
            services.Configure<IdentityServerConfig>(config.GetSection(nameof(IdentityServerConfig)));

            return services;
        }
        
        public static void SetOpenIdConnectOptions(this OpenIdConnectOptions options, CustomOpenIdConnectOptions existedOptions)
        {
            options.ClientId = existedOptions.ClientId;
            options.ClientSecret = existedOptions.ClientSecret;
            options.Authority = existedOptions.Authority;
            options.ClaimsIssuer = existedOptions.ClaimsIssuer;
            options.MetadataAddress = existedOptions.MetadataAddress;
            options.GetClaimsFromUserInfoEndpoint = existedOptions.GetClaimsFromUserInfoEndpoint;
            options.SignInScheme = existedOptions.SignInScheme;
            options.ResponseType = existedOptions.ResponseType;
            options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
        }
    }
}
