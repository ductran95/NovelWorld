using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Storage.Configurations;

namespace NovelWorld.Storage.AzureBlob.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAzureBlob(this IServiceCollection services, StorageConfiguration storageConfiguration)
        {
            
            return services;
        }
    }
}
