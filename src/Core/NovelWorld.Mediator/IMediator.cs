using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace NovelWorld.Mediator
{
    public interface IMediator : MediatR.IMediator
    {
        //
        // Summary:
        //     Asynchronously send a notification to multiple handlers
        //
        // Parameters:
        //   notification:
        //     Notification object
        //
        //   cancellationToken:
        //     Optional cancellation token
        //
        // Returns:
        //     A task that represents the publish operation.
        Task Publish(object notification, PublishStrategy strategy, CancellationToken cancellationToken = default);
        //
        // Summary:
        //     Asynchronously send a notification to multiple handlers
        //
        // Parameters:
        //   notification:
        //     Notification object
        //
        //   cancellationToken:
        //     Optional cancellation token
        //
        // Returns:
        //     A task that represents the publish operation.
        Task Publish<TNotification>(TNotification notification, PublishStrategy strategy, CancellationToken cancellationToken = default) where TNotification : INotification;
    }
}
