using System.Linq;
using Microsoft.AspNetCore.Http;

namespace NovelWorld.API.Extensions
{
    public static class HttpContextExtensions
    {
        public static bool IsFromLocal(this HttpContext httpContext)
        {
            var localAddresses = new string[] { "127.0.0.1", "::1", httpContext.Connection.LocalIpAddress.ToString() };
            if (!localAddresses.Contains(httpContext.Connection.RemoteIpAddress.ToString()))
            {
                return false;
            }

            return true;
        }
    }
}