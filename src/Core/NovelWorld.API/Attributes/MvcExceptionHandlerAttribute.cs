using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Filters;

namespace NovelWorld.API.Attributes
{
    public class MvcExceptionHandlerAttribute : ExceptionFilterAttribute
    {
        public MvcExceptionHandlerAttribute()
        {
        }

        public override void OnException(ExceptionContext context)
        {
            var logger = context.HttpContext.RequestServices
                .GetService<ILogger<MvcExceptionHandlerAttribute>>();
            
            var options = context.HttpContext.RequestServices
                .GetService<MvcExceptionHandlerOptions>();
            
            var modelMetadataProvider = context.HttpContext.RequestServices
                .GetService<IModelMetadataProvider>();
            
            MvcExceptionHandlerFilter.OnExceptionInternal(context, logger, options, modelMetadataProvider);
        }
    }
}