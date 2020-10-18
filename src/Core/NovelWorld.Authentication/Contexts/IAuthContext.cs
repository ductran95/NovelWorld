using System.Net;
using NovelWorld.Data.DTO.Auth;

namespace NovelWorld.Authentication.Contexts
{
    public interface IAuthContext
    {
        AuthenticatedUser User { get; }
        IPAddress IP { get; }
    }
}