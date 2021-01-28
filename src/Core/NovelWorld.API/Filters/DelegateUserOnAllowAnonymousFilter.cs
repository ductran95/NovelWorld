using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.API.Contexts.Implements;
using NovelWorld.Authentication.Contexts.Abstractions;

namespace NovelWorld.API.Filters
{
    public class DelegateUserOnAllowAnonymousFilter: IActionFilter, IOrderedFilter
    {
        // Run first
        public int Order => 0;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            OnActionExecutingInternal(context);
        }

        internal static void OnActionExecutingInternal(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor action)
            {
                var allowAnonymous = action.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>(true);
                if (allowAnonymous != null)
                {
                    var authContext = context.HttpContext.RequestServices.GetRequiredService<IAuthContext>();
                    if (authContext != null && authContext is HttpAuthContext httpAuthContext)
                    {
                        httpAuthContext.SetDefaultUser();
                    }
                }
            }
        }
    }
}