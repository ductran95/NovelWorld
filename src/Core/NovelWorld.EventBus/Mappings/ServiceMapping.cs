using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NovelWorld.EventBus.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultEventBusSubscriptionsManager(this IServiceCollection services)
        {
            services.TryAddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            
            return services;
        }
        
        public static IServiceCollection RegisterDefaultEventBus(this IServiceCollection services)
        {
            services.TryAddSingleton<IEventBus, NullEventBus>();
            
            return services;
        }
    }
}
