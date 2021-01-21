using MediatR;

namespace NovelWorld.Domain.Commands
{
    public abstract class Command<T>: IRequest<T>
    {
    }

    public abstract class Command : IRequest<bool>
    {
    }
}
