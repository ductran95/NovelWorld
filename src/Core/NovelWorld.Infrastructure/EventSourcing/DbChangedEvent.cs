using NovelWorld.EventBus.Events;

namespace NovelWorld.Infrastructure.EventSourcing
{
    public class DbChangedEvent: IntegrationEvent
    {
        public string Name { get; set; }
        public string Action { get; set; }
        public object Data { get; set; }
    }
}