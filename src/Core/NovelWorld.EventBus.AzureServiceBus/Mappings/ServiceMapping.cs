using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NovelWorld.EventBus.Configurations;

namespace NovelWorld.EventBus.AzureServiceBus.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterAzureServiceBus(this IServiceCollection services, EventBusConfiguration eventBusConfig)
        {
            var subscriptionClientName = eventBusConfig.SubscriptionClientName;
            
            services.TryAddSingleton<IServiceBusPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersistentConnection>>();

                var serviceBusConnectionString = eventBusConfig.EventBusConnection;
                var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                return new DefaultServiceBusPersistentConnection(serviceBusConnection, logger);
            });

            services.TryAddSingleton<IEventBus>(sp =>
            {
                var serviceBusPersistentConnection = sp.GetRequiredService<IServiceBusPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusServiceBus(serviceBusPersistentConnection, logger, sp,
                    eventBusSubscriptionsManager, subscriptionClientName);
            });
            
            return services;
        }
    }
}
