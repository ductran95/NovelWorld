namespace NovelWorld.Domain.Commands
{
    public interface ICommand<out T>: MediatR.IRequest<T>
    {
    }

    public interface ICommand : ICommand<bool>
    {
    }
}
