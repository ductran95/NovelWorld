using System.Collections.Generic;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Domain.Proxies;
using NovelWorld.Mediator;
using NovelWorld.Mediator.Pipeline;

namespace NovelWorld.Domain.Mappings
{
    public static class BaseMediatorMapping
    {
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
    }
}
