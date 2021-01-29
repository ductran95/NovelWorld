using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Storage.AzureBlob.Providers.Implements;
using NovelWorld.Storage.Configurations;
using NovelWorld.Storage.Providers.Abstractions;

namespace NovelWorld.Storage.AzureBlob.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAzureBlob(this IServiceCollection services)
        {
            services.TryAddSingleton<IStorageProvider, AzureBlobStorageProvider>();
            
            return services;
        }
    }
}
