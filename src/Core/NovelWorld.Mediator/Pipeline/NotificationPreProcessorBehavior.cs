using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovelWorld.Mediator.Pipeline
{
    /// <summary>
    /// Behavior for executing all <see cref="IRequestPreProcessor{TRequest}"/> instances before handling a request
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public class NotificationPreProcessorBehavior<TRequest> : INotificationPipelineBehavior<TRequest>
        where TRequest : notnull
    {
        private readonly IEnumerable<INotificationPreProcessor<TRequest>> _preProcessors;

        public NotificationPreProcessorBehavior(IEnumerable<INotificationPreProcessor<TRequest>> preProcessors)
        {
            _preProcessors = preProcessors;
        }

        public async Task Handle(TRequest request, CancellationToken cancellationToken, NotificationHandlerDelegate next)
        {
            foreach (var processor in _preProcessors)
            {
                await processor.Process(request, cancellationToken).ConfigureAwait(false);
            }

            await next().ConfigureAwait(false);
        }
    }
}
