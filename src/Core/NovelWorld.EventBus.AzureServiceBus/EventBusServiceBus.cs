using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NovelWorld.EventBus.EventHandlers;
using NovelWorld.EventBus.Events;

namespace NovelWorld.EventBus.AzureServiceBus
{
    public class EventBusServiceBus : IEventBus
    {
        private static readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        private readonly IServiceBusPersistentConnection _serviceBusPersistentConnection;
        private readonly ILogger<EventBusServiceBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IServiceProvider _serviceProvider;
        private const string IntegrationEventSuffix = "IntegrationEvent";

        public EventBusServiceBus(IServiceBusPersistentConnection serviceBusPersistentConnection,
            ILogger<EventBusServiceBus> logger, IServiceProvider serviceProvider, IEventBusSubscriptionsManager subsManager, string subscriptionClientName)
        {
            _serviceBusPersistentConnection = serviceBusPersistentConnection;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();

            _subscriptionClient = new SubscriptionClient(serviceBusPersistentConnection.ServiceBusConnectionStringBuilder,
                subscriptionClientName);
            _serviceProvider = serviceProvider;

            RemoveDefaultRule();
            RegisterSubscriptionClientMessageHandler();
        }

        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(IntegrationEventSuffix, "");
            var jsonMessage = JsonConvert.SerializeObject(@event, _jsonSerializerSettings);
            var body = Encoding.UTF8.GetBytes(jsonMessage);

            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = body,
                Label = eventName,
            };

            var topicClient = _serviceBusPersistentConnection.CreateModel();

            topicClient.SendAsync(message)
                .GetAwaiter()
                .GetResult();
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : DynamicIntegrationEventHandler
        {
            _logger.LogInformation("Subscribing to dynamic event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(IntegrationEventSuffix, "");

            var containsKey = _subsManager.HasSubscriptionsForEvent<T>();
            if (!containsKey)
            {
                try
                {
                    _subscriptionClient.AddRuleAsync(new RuleDescription
                    {
                        Filter = new CorrelationFilter { Label = eventName },
                        Name = eventName
                    }).GetAwaiter().GetResult();
                }
                catch (ServiceBusException)
                {
                    _logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
                }
            }

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(IntegrationEventSuffix, "");

            try
            {
                _subscriptionClient
                 .RemoveRuleAsync(eventName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
            }

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : DynamicIntegrationEventHandler
        {
            _logger.LogInformation("Unsubscribing from dynamic event {EventName}", eventName);

            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            _subsManager.Clear();
        }

        private void RegisterSubscriptionClientMessageHandler()
        {
            _subscriptionClient.RegisterMessageHandler(
                async (message, token) =>
                {
                    var eventName = $"{message.Label}{IntegrationEventSuffix}";
                    var messageData = Encoding.UTF8.GetString(message.Body);

                    // Complete the message so that it is not received again.
                    if (await ProcessEvent(eventName, messageData, token))
                    {
                        await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
                    }
                },
                new MessageHandlerOptions(ExceptionReceivedHandler) { MaxConcurrentCalls = 10, AutoComplete = false });
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            var ex = exceptionReceivedEventArgs.Exception;
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - Context: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        private async Task<bool> ProcessEvent(string eventName, string message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Processing Service Bus event: {EventName}", eventName);

            var processed = false;
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        {
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType) as DynamicIntegrationEventHandler;
                            if (handler == null) continue;
                            dynamic eventData = JObject.Parse(message);
                            await handler.Handle(eventData, cancellationToken);
                        }
                        else
                        {
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType);
                            if (handler == null) continue;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonConvert.DeserializeObject(message, eventType, _jsonSerializerSettings);
                            var concreteType = typeof(IntegrationEventHandler<>).MakeGenericType(eventType);
                            // ReSharper disable once PossibleNullReferenceException
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { integrationEvent, cancellationToken });
                        }
                    }
                }
                processed = true;

                _logger.LogInformation("Done processing Service Bus event: {EventName}", eventName);
            }
            else
            {
                _logger.LogWarning("No subscription for Service Bus event: {EventName}", eventName);
            }
            return processed;
        }

        private void RemoveDefaultRule()
        {
            try
            {
                _subscriptionClient
                 .RemoveRuleAsync(RuleDescription.DefaultRuleName)
                 .GetAwaiter()
                 .GetResult();
            }
            catch (MessagingEntityNotFoundException)
            {
                _logger.LogWarning("The messaging entity {DefaultRuleName} Could not be found.", RuleDescription.DefaultRuleName);
            }
        }
    }
}
