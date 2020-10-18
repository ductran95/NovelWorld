using System.Collections.Generic;
using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.EventSourcing
{
    public interface IEventSource
    {
        IEnumerable<DbChangedEvent> EventList { get; set; }
        void Add(DbChangedEvent @event);
        void Remove(DbChangedEvent @event);
        void Publish();
        Task PublishAsync();
    }
}