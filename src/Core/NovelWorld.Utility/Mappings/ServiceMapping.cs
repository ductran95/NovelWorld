using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Utility.Helpers.Implements;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace NovelWorld.Utility.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultHelpers(this IServiceCollection services)
        {
            services.TryAddSingleton(_ => new RestClient().UseNewtonsoftJson(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
            services.TryAddSingleton<IApiClient, RestsharpApiClient>();
            services.TryAddSingleton<IPasswordHasher, PBKDF2PasswordHasher>();
            services.TryAddSingleton<ICryptoHelper, AESCryptoHelper>();

            return services;            
        }
    }
}
