using Microsoft.Extensions.DependencyInjection;
using NovelWorld.API.Contexts;
using NovelWorld.Authentication.Contexts;

namespace NovelWorld.API.Mappings
{
    public static class APIServiceMapping
    {
        public static IServiceCollection RegisterAuthContext(this IServiceCollection services)
        {
            services.AddScoped<IAuthContext, HttpAuthContext>();

            return services;
        }
    }
}
