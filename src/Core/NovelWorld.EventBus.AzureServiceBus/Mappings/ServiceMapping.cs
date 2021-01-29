using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelWorld.EventBus.AzureServiceBus.Bus.Implements;
using NovelWorld.EventBus.AzureServiceBus.Connections.Abstractions;
using NovelWorld.EventBus.AzureServiceBus.Connections.Implements;
using NovelWorld.EventBus.Bus.Abstractions;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.SubscriptionsManagers.Abstractions;

namespace NovelWorld.EventBus.AzureServiceBus.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAzureServiceBus(this IServiceCollection services)
        {
            services.TryAddSingleton<ServiceBusConnectionStringBuilder>(sp =>
            {
                // ReSharper disable once PossibleNullReferenceException
                var eventBusConfig = sp.GetService<IOptions<EventBusConfiguration>>().Value;
                
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersistentConnection>>();

                var serviceBusConnectionString = eventBusConfig.Connection;
                return new ServiceBusConnectionStringBuilder(serviceBusConnectionString);
            });

            services.TryAddSingleton<IServiceBusPersistentConnection, DefaultServiceBusPersistentConnection>();
            services.TryAddSingleton<IEventBus, EventBusServiceBus>();
            
            return services;
        }
    }
}
