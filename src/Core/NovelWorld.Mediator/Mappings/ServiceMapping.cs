using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NovelWorld.Mediator.Mappings
{
    public static class ServiceMapping
    {
        public static IServiceCollection RegisterDefaultPublishStrategies(this IServiceCollection services)
        {
            services.TryAddSingleton(sp => new Dictionary<PublishStrategy, Mediator.IPublisher>
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
