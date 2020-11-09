using MediatR;

namespace NovelWorld.Domain.Events
{
    public abstract class Event: INotification
    {
        //public Event()
        //{
        //    Id = Guid.NewGuid();
        //    CreationDate = DateTime.UtcNow;
        //}

        //[JsonConstructor]
        //public Event(Guid id, DateTime createDate)
        //{
        //    Id = id;
        //    CreationDate = createDate;
        //}

        //[JsonProperty]
        //public Guid Id { get; set; }

        //[JsonProperty]
        //public DateTime CreationDate { get; set; }
    }
}
