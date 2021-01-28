using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Filters;

namespace NovelWorld.API.Attributes
{
    public class ApiExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public ApiExceptionHandlerAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetService<ILogger<ApiExceptionHandlerAttribute>>();
            
            ApiExceptionHandlerFilter.OnExceptionInternal(context, logger);
        }
    }
}