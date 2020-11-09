using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NovelWorld.API.Contexts;
using NovelWorld.Authentication.Contexts.Implements;

namespace NovelWorld.API.Attributes
{
    public class DelegateUserOnAllowAnonymousAttribute : ActionFilterAttribute
    {
        // Run first
        public DelegateUserOnAllowAnonymousAttribute()
        {
            Order = 0;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
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