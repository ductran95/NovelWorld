using NovelWorld.EventBus.EventHandlers;
using NovelWorld.EventBus.Events;

namespace NovelWorld.EventBus
{
    public class NullEventBus : IEventBus
    {
        //private static JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        //{
        //    MaxDepth = 3,
        //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //};

        //private readonly ILogger<NullEventBus> _logger;

        //public NullEventBus(ILogger<NullEventBus> logger)
        //{
        //    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        //}

        public void Publish(IntegrationEvent @event)
        {
            //try
            //{
            //    var message = JsonConvert.SerializeObject(@event, _jsonSerializerSettings);
            //    _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);
            //}
            //catch(Exception ex)
            //{
            //    _logger.LogWarning(ex, "Could not publish event: {EventId} ({ExceptionMessage})", @event.Id, ex.Message);
            //}
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : DynamicIntegrationEventHandler
        {
            
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : DynamicIntegrationEventHandler
        {
           
        }
    }
}
