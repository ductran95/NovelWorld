using NovelWorld.EventBus.EventHandlers;
using NovelWorld.EventBus.Events;

namespace NovelWorld.EventBus.Bus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent @event);

        void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>;

        void SubscribeDynamic<TH>(string eventName)
            where TH : DynamicIntegrationEventHandler;

        void UnsubscribeDynamic<TH>(string eventName)
            where TH : DynamicIntegrationEventHandler;

        void Unsubscribe<T, TH>()
            where TH : IntegrationEventHandler<T>
            where T : IntegrationEvent;
    }
}
