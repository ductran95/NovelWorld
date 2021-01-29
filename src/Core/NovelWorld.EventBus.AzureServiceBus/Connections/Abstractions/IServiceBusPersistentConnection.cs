using System;
using Microsoft.Azure.ServiceBus;

namespace NovelWorld.EventBus.AzureServiceBus.Connections.Abstractions
{
    public interface IServiceBusPersistentConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
