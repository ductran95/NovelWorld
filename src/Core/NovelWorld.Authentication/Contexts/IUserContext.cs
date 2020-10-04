using NovelWorld.Data.Entities.Auth;

namespace NovelWorld.Authentication.Contexts
{
    public interface IUserContext
    {
        User User { get; }
    }
}