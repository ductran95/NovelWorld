using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.API.Contexts.Implements;
using NovelWorld.Authentication.Contexts.Abstractions;

namespace NovelWorld.API.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterHttpAuthContext(this IServiceCollection services)
        {
            services.TryAddScoped<IAuthContext, HttpAuthContext>();

            return services;
        }
    }
}
