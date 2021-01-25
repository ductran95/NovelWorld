using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Authentication;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Configurations;
using NovelWorld.Domain.Proxies;
using NovelWorld.EventBus;
using NovelWorld.EventBus.AzureServiceBus.Mappings;
using NovelWorld.EventBus.Mappings;
using NovelWorld.EventBus.RabbitMQ.Mappings;
using NovelWorld.Mediator;
using NovelWorld.Mediator.Mappings;
using NovelWorld.Mediator.Pipeline;
using NovelWorld.Storage;

namespace NovelWorld.Domain.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultMediator(this IServiceCollection services)
        {
            services.RegisterDefaultProxies()
                    .RegisterDefaultPublishStrategies();
            
            return services;
        }
        
        public static IServiceCollection RegisterDefaultProxies(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(INotificationPipelineBehavior<>), typeof(NotificationPreProcessorBehavior<>));
            services.TryAddTransient(typeof(INotificationPreProcessor<>), typeof(ValidationProxy<>));
            services.TryAddTransient(typeof(INotificationPipelineBehavior<>), typeof(LoggingProxy<>));
            services.TryAddTransient(typeof(IRequestPreProcessor<>), typeof(ValidationProxy<>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingProxy<,>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizeProxy<,>));
            services.TryAddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkProxy<,>));

            return services;
        }
        
        public static IServiceCollection RegisterDefaultAppConfig(
            this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config);
            services.Configure<OAuth2Configuration>(config.GetSection(nameof(OAuth2Configuration)));
            services.Configure<EventBusConfiguration>(config.GetSection(nameof(EventBusConfiguration)));
            services.Configure<StorageConfiguration>(config.GetSection(nameof(StorageConfiguration)));

            return services;
        }
        
        public static IServiceCollection RegisterDefaultEventBus(this IServiceCollection services, EventBusConfiguration eventBusConfig)
        {
            services.RegisterDefaultEventBusSubscriptionsManager();

            switch (eventBusConfig.Type)
            {
                case EventBusTypes.RabbitMQ:
                    services.RegisterRabbitMQ(eventBusConfig);

                    break;

                 case EventBusTypes.AzureServiceBus:
                     services.RegisterAzureServiceBus(eventBusConfig);

                    break;

                default:
                    services.RegisterDefaultEventBus();
                    break;
            }

            return services;
        }
    }
}
