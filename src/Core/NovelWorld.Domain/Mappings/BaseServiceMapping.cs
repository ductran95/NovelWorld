using System.Collections.Generic;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NovelWorld.Common.Helpers.Abstractions;
using NovelWorld.Common.Helpers.Implements;
using NovelWorld.Data.Configurations;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Proxies;
using NovelWorld.EventBus;
using NovelWorld.EventBus.AzureServiceBus;
using NovelWorld.EventBus.RabbitMQ;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;
using NovelWorld.Infrastructure.EventSourcing.Implements;
using NovelWorld.Mediator;
using NovelWorld.Mediator.Pipeline;
using RabbitMQ.Client;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace NovelWorld.Domain.Mappings
{
    public static class BaseServiceMapping
    {
        public static IServiceCollection RegisterBaseHelpers(this IServiceCollection services)
        {
            services.AddSingleton(sp => new RestClient().UseNewtonsoftJson(new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
            services.AddSingleton<IApiClient, ApiClient>();

            return services;
        }
        public static IServiceCollection RegisterBaseProxies(this IServiceCollection services)
        {
            services.AddTransient(typeof(INotificationPipelineBehavior<>), typeof(NotificationPreProcessorBehavior<>));
            services.AddTransient(typeof(INotificationPreProcessor<>), typeof(ValidationProxy<>));
            services.AddTransient(typeof(INotificationPipelineBehavior<>), typeof(LoggingProxy<>));
            services.AddTransient(typeof(IRequestPreProcessor<>), typeof(ValidationProxy<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingProxy<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizeProxy<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkProxy<,>));

            return services;
        }

        public static IServiceCollection RegisterPublishStrategies(this IServiceCollection services)
        {
            services.AddSingleton(sp => new Dictionary<PublishStrategy, Mediator.IPublisher>
            {
                { PublishStrategy.SyncContinueOnException, new PublisherSyncContinueOnException(sp) },
                { PublishStrategy.SyncStopOnException, new PublisherSyncStopOnException(sp) },
                { PublishStrategy.Async, new PublisherAsyncContinueOnException(sp) },
                { PublishStrategy.ParallelNoWait, new PublisherParallelNoWait(sp) },
                { PublishStrategy.ParallelWhenAll, new PublisherParallelWhenAll(sp) },
                { PublishStrategy.ParallelWhenAny, new PublisherParallelWhenAny(sp) },
            });

            return services;
        }
        
        public static IServiceCollection RegisterBaseEventSourcing(this IServiceCollection services)
        {
            services.AddScoped<IDbEventSource, DbEventSource>();

            return services;
        }
        
        public static IServiceCollection RegisterBaseEventBus(this IServiceCollection services, EventBusConfig eventBusConfig)
        {
            var subscriptionClientName = eventBusConfig.SubscriptionClientName;

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            switch (eventBusConfig.Type)
            {
                case EventBusTypes.RabbitMQ:
                    services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
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

                    services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
                    {
                        var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
                        var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                        var retryCount = eventBusConfig.EventBusRetryCount;

                        return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
                    });

                    break;

                 case EventBusTypes.AzureServiceBus:
                    services.AddSingleton<IServiceBusPersistentConnection>(sp =>
                    {
                        var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersistentConnection>>();

                        var serviceBusConnectionString = eventBusConfig.EventBusConnection;
                        var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                        return new DefaultServiceBusPersistentConnection(serviceBusConnection, logger);
                    });

                    services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                    {
                        var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersistentConnection>();
                        var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                        var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                        return new EventBusServiceBus(serviceBusPersisterConnection, logger, sp,
                            eventBusSubcriptionsManager, subscriptionClientName);
                    });

                    break;

                default:
                    services.AddSingleton<IEventBus, NullEventBus>();
                    break;
            }

            return services;
        }
    }
}
