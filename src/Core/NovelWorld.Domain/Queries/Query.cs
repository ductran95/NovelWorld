using MediatR;

namespace NovelWorld.Domain.Queries
{
    public abstract class Query<T>: IRequest<T>, ICanSwallowException
    {
        public bool SwallowException { get; set; } = false;
    }

    public abstract class Query : Query<bool>
    {
    }
}
