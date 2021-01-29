using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.Utility;

namespace NovelWorld.API.Extensions
{
    public static class HttpContextExtensions
    {
        private static readonly RouteData EmptyRouteData = new RouteData();
        private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();
        
        public static bool IsFromLocal(this HttpContext httpContext)
        {
            var localAddresses = new string[] { "127.0.0.1", "::1", httpContext.Connection.LocalIpAddress.ToString() };
            if (!localAddresses.Contains(httpContext.Connection.RemoteIpAddress.ToString()))
            {
                return false;
            }

            return true;
        }

        public static async Task WriteActionResultAsync(this HttpContext httpContext, ViewResult result, RouteData routeData = null)
        {
            Check.NotNull(httpContext);
            Check.NotNull(result);

            var executor = httpContext.RequestServices.GetService<IActionResultExecutor<ViewResult>>();
            Check.NotNull(executor);

            var newRouteData = routeData ?? httpContext.GetRouteData() ?? EmptyRouteData;
            var actionContext = new ActionContext(httpContext, newRouteData, EmptyActionDescriptor, result.ViewData.ModelState);
            // ReSharper disable once PossibleNullReferenceException
            await executor.ExecuteAsync(actionContext, result);
        }
    }
}