using System.Net;
using NovelWorld.Authentication.DTO;

namespace NovelWorld.Authentication.Contexts.Abstractions
{
    public interface IAuthContext
    {
        AuthenticatedUser User { get; }
        IPAddress IP { get; }
    }
}