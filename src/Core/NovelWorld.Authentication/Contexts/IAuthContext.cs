using System.Collections.Generic;
using System.Net;
using NovelWorld.Data.Entities.Auth;

namespace NovelWorld.Authentication.Contexts
{
    public interface IAuthContext
    {
        User User { get; }
        IEnumerable<string> Roles { get; }
        IPAddress IP { get; }
    }
}