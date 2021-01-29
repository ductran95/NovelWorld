using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NovelWorld.Domain.Proxies;
using NovelWorld.Mediator;
using NovelWorld.Mediator.Mappings;
using NovelWorld.Mediator.Pipeline;

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
    }
}
