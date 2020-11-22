using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace NovelWorld.Mediator
{
    internal abstract class NotificationHandlerWrapper
    {
        public abstract Task Handle(IPublisher publisher, INotification notification, CancellationToken cancellationToken,
                                    Func<IPublisher, IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, IServiceScope, CancellationToken, Task> publish);
    }

    internal class NotificationHandlerWrapperImpl<TNotification> : NotificationHandlerWrapper
        where TNotification : INotification
    {
        public override Task Handle(IPublisher publisher, INotification notification, CancellationToken cancellationToken,
                                    Func<IPublisher, IEnumerable<Func<INotification, CancellationToken, Task>>, INotification, IServiceScope, CancellationToken, Task> publish)
        {
            var scope = publisher.GetServiceScope();
            var serviceFactory = publisher.GetServiceFactory(scope);

            var handlers = serviceFactory
                .GetInstances<INotificationHandler<TNotification>>()
                .Select(x => new Func<INotification, CancellationToken, Task>((theNotification, theToken) => x.Handle((TNotification)theNotification, theToken)));

            Task NotificationHandler() => publish(publisher, handlers, notification, scope, cancellationToken);

            return serviceFactory
                .GetInstances<INotificationPipelineBehavior<INotification>>()
                .Reverse()
                .Aggregate((NotificationHandlerDelegate)NotificationHandler, (next, pipeline) => () => pipeline.Handle(notification, cancellationToken, next))();

        }
    }
}
