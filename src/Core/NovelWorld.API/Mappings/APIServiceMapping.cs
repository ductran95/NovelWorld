using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.API.Contexts;
using NovelWorld.API.Contexts.Implements;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Data.Configurations;

namespace NovelWorld.API.Mappings
{
    public static class APIServiceMapping
    {
        public static IServiceCollection RegisterAuthContext(this IServiceCollection services)
        {
            services.AddScoped<IAuthContext, HttpAuthContext>();

            return services;
        }
        
        public static IServiceCollection AddBaseAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.Configure<UrlConfig>(config.GetSection(nameof(UrlConfig)));
            services.Configure<AttachmentConfig>(config.GetSection(nameof(AttachmentConfig)));
            services.Configure<OAuth2Config>(config.GetSection(nameof(OAuth2Config)));
            services.Configure<EventBusConfig>(config.GetSection(nameof(EventBusConfig)));

            return services;
        }
    }
}
