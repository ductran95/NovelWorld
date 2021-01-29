using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Storage.Configurations;
using NovelWorld.Storage.Local.Providers.Implements;
using NovelWorld.Storage.Providers.Abstractions;

namespace NovelWorld.Storage.Local.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterLocalStorage(this IServiceCollection services)
        {
            services.TryAddSingleton<IStorageProvider, LocalStorageProvider>();
            
            return services;
        }
    }
}
