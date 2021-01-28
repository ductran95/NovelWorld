using Microsoft.AspNetCore.Mvc.Filters;
using NovelWorld.API.Filters;

namespace NovelWorld.API.Attributes
{
    public class RequestValidationAttribute : ActionFilterAttribute
    {
        public RequestValidationAttribute()
        {
            Order = 1;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RequestValidationFilter.OnActionExecutingInternal(context);
        }
    }
}