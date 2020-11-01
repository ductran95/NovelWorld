using System.Net;
using NovelWorld.Authentication.DTO;

namespace NovelWorld.Authentication.Contexts.Implements
{
    public interface IAuthContext
    {
        AuthenticatedUser User { get; }
        IPAddress IP { get; }
    }
}