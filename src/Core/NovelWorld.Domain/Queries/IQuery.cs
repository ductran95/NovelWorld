namespace NovelWorld.Domain.Queries
{
    public interface IQuery<out T>: MediatR.IRequest<T>
    {
    }

    public interface IQuery : IQuery<bool>
    {
    }
}
