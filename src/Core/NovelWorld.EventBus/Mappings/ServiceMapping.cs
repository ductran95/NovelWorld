using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.EventBus.Bus.Abstractions;
using NovelWorld.EventBus.Bus.Implements;
using NovelWorld.EventBus.SubscriptionsManagers.Abstractions;
using NovelWorld.EventBus.SubscriptionsManagers.Implements;

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
