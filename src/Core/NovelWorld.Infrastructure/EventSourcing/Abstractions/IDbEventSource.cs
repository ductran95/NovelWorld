using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.EventSourcing.Abstractions
{
    public interface IDbEventSource
    {
        IEnumerable<DbChangedEvent> EventList { get; set; }
        void Add(DbChangedEvent @event);
        void Remove(DbChangedEvent @event);
        void Publish();
        Task PublishAsync(CancellationToken cancellationToken = default);
    }
}