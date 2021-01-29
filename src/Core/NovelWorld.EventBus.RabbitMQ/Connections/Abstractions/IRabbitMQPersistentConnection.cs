using System;
using RabbitMQ.Client;

namespace NovelWorld.EventBus.RabbitMQ.Connections.Abstractions
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
