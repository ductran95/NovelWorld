using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelWorld.EventBus.Bus.Abstractions;
using NovelWorld.EventBus.Configurations;
using NovelWorld.EventBus.RabbitMQ.Bus.Implements;
using NovelWorld.EventBus.RabbitMQ.Connections.Abstractions;
using NovelWorld.EventBus.RabbitMQ.Connections.Implements;
using RabbitMQ.Client;

namespace NovelWorld.EventBus.RabbitMQ.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterRabbitMQ(this IServiceCollection services)
        {
            services.TryAddSingleton<IConnectionFactory>(sp =>
            {
                // ReSharper disable once PossibleNullReferenceException
                var eventBusConfig = sp.GetService<IOptions<EventBusConfiguration>>().Value;
                return new ConnectionFactory()
                {
                    Uri = new Uri(eventBusConfig.ConnectionString),
                    DispatchConsumersAsync = true
                };
            });
                
            services.TryAddSingleton<IRabbitMQPersistentConnection, DefaultRabbitMQPersistentConnection>();
            
            services.TryAddSingleton<IEventBus, EventBusRabbitMQ>();
            
            return services;
        }
    }
}
