using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NovelWorld.EventBus;
using NovelWorld.Infrastructure.EventSourcing.Abstractions;

namespace NovelWorld.Infrastructure.EventSourcing.Implements
{
    public class DbEventSource: IDbEventSource
    {
        private readonly IEventBus _eventBus;
        public IEnumerable<DbChangedEvent> EventList { get; set; }

        public DbEventSource(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }
        
        public void Add(DbChangedEvent @event)
        {
            var eventList = EventList.ToList();
            eventList.Add(@event);
            EventList = eventList;
        }

        public void Remove(DbChangedEvent @event)
        {
            var eventList = EventList.ToList();
            eventList.Remove(@event);
            EventList = eventList;
        }

        public void Publish()
        {
            foreach (var @event in EventList)
            {
                _eventBus.Publish(@event);
            }
        }

        public Task PublishAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}