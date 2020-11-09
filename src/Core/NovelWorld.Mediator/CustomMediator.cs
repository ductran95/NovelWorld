using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace NovelWorld.Mediator
{
    public class CustomMediator : MediatR.Mediator, IMediator
    {
        private static readonly ConcurrentDictionary<Type, NotificationHandlerWrapper> _notificationHandlers =
            new ConcurrentDictionary<Type, NotificationHandlerWrapper>();
        
        private readonly Dictionary<PublishStrategy, IPublisher> _publishers;

        public CustomMediator(ServiceFactory serviceFactory, Dictionary<PublishStrategy, IPublisher> publishers) : base(serviceFactory)
        {
            _publishers = publishers;
        }

        public new Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification
        {
            return Publish(notification, PublishStrategy.ParallelNoWait, cancellationToken);
        }

        public new Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return Publish(notification, PublishStrategy.ParallelNoWait, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            var publisher = _publishers[strategy];

            return PublishNotification(publisher, notification, cancellationToken);
        }

        public Task Publish(object notification, PublishStrategy strategy, CancellationToken cancellationToken = default)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            if (notification is INotification instance)
            {
                var publisher = _publishers[strategy];

                return PublishNotification(publisher, instance, cancellationToken);
            }

            throw new ArgumentException($"{nameof(notification)} does not implement ${nameof(INotification)}");
        }

        private async Task PublishCore(IPublisher publisher, IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, IServiceScope scope, CancellationToken cancellationToken)
        {
            await publisher.Publish(allHandlers, notification, scope, cancellationToken);
        }

        private Task PublishNotification(IPublisher publisher, INotification notification, CancellationToken cancellationToken = default)
        {
            var notificationType = notification.GetType();
            var handler = _notificationHandlers.GetOrAdd(notificationType,
                t => (NotificationHandlerWrapper)Activator.CreateInstance(typeof(NotificationHandlerWrapperImpl<>).MakeGenericType(notificationType)));

            return handler.Handle(publisher, notification, cancellationToken, PublishCore);
        }
    }
}
