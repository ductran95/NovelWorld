using MediatR;

namespace NovelWorld.Domain.Commands
{
    public abstract class Command<T>: IRequest<T>
    {
        //public Command()
        //{
        //    Id = Guid.NewGuid();
        //    CreationDate = DateTime.UtcNow;
        //}

        //[JsonConstructor]
        //public Command(Guid id, DateTime createDate)
        //{
        //    Id = id;
        //    CreationDate = createDate;
        //}

        //[JsonProperty]
        //public Guid Id { get; set; }

        //[JsonProperty]
        //public DateTime CreationDate { get; set; }
    }

    public abstract class Command : IRequest<bool>
    {
        //public Command()
        //{
        //    Id = Guid.NewGuid();
        //    CreationDate = DateTime.UtcNow;
        //}

        //[JsonConstructor]
        //public Command(Guid id, DateTime createDate)
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
