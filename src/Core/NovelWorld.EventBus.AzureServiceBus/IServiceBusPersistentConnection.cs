using System;
using Microsoft.Azure.ServiceBus;

namespace NovelWorld.EventBus.AzureServiceBus
{
    public interface IServiceBusPersistentConnection : IDisposable
    {
        ServiceBusConnectionStringBuilder ServiceBusConnectionStringBuilder { get; }

        ITopicClient CreateModel();
    }
}
