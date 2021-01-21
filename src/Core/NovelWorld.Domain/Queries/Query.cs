using MediatR;

namespace NovelWorld.Domain.Queries
{
    public abstract class Query<T>: IRequest<T>
    {
    }

    public abstract class Query : IRequest<bool>
    {
    }
}
