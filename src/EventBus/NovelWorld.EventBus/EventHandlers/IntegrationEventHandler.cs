using System.Threading.Tasks;
using NovelWorld.EventBus.Events;

namespace NovelWorld.EventBus.EventHandlers
{
    public interface IntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
