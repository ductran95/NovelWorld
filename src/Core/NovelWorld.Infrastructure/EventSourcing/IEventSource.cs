using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.EventSourcing
{
    public interface IEventSource
    {
        void Add(DbChangedEvent @eEvent);
        void Remove(DbChangedEvent @eEvent);
        void Publish();
        Task PublishAsync();
    }
}