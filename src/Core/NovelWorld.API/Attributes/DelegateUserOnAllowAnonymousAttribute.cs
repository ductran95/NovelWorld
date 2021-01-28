using Microsoft.AspNetCore.Mvc.Filters;
using NovelWorld.API.Filters;

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
            DelegateUserOnAllowAnonymousFilter.OnActionExecutingInternal(context);
        }
    }
}