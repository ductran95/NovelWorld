using MediatR;
using NovelWorld.Utility;

namespace NovelWorld.Domain.Commands
{
    public abstract class Command<T>: IRequest<T>, ICanSwallowException
    {
        public bool SwallowException { get; set; } = true;
    }

    public abstract class Command : Command<bool>
    {
    }
}
