using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace NovelWorld.EventBus.RabbitMQ.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, EventBusConfiguration eventBusConfig)
        {
            var subscriptionClientName = eventBusConfig.SubscriptionClientName;
            
            services.TryAddSingleton<IRabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = eventBusConfig.EventBusConnection,
                    DispatchConsumersAsync = true
                };

                if (!string.IsNullOrEmpty(eventBusConfig.EventBusUserName))
                {
                    factory.UserName = eventBusConfig.EventBusUserName;
                }

                if (!string.IsNullOrEmpty(eventBusConfig.EventBusPassword))
                {
                    factory.Password = eventBusConfig.EventBusPassword;
                }

                var retryCount = eventBusConfig.EventBusRetryCount;

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
            
            services.TryAddSingleton<IEventBus>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = eventBusConfig.EventBusRetryCount;

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, subscriptionClientName, retryCount);
            });
            
            return services;
        }
    }
}
