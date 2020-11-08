using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Extensions;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.API.Filters
{
    public class HttpModelExceptionFilter : IExceptionFilter
    {
        public string FallbackView { get; set; } = "Views/Home/Index";
        public int Order { get; set; } = 10;
        private readonly ILogger<HttpModelExceptionFilter> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public HttpModelExceptionFilter(
            ILogger<HttpModelExceptionFilter> logger,
            IModelMetadataProvider modelMetadataProvider
            )
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception == null)
            {
                return;
            }

            HttpException exceptionToHandle;

            if (exception is HttpException httpException)
            {
                exceptionToHandle = httpException;
            }
            else if (exception is DomainException domainException)
            {
                exceptionToHandle = domainException.WrapException();
            }
            else
            {
                var errorResponse = new List<Error> { new Error(CommonErrorCodes.InternalServerError, exception.Message) };

                exceptionToHandle = new HttpException(HttpStatusCode.InternalServerError, errorResponse, exception.Message, exception);
            }

            // ReSharper disable once PossibleNullReferenceException
            _logger.LogError(exceptionToHandle, exceptionToHandle.InnerException.Message);

            var currentViewName = context.GetViewName();
            var viewEngine = context.HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();
            var result = new ViewResult {ViewName = currentViewName};
                
            if (!viewEngine.GetView(null, result.ViewName, true).Success && !viewEngine.FindView(context, result.ViewName, true).Success)
            {
                result.ViewName = FallbackView;
            }
                
            result.ViewData = new ViewDataDictionary(_modelMetadataProvider,
                context.ModelState);
            result.ViewData.Add("Exception", context.Exception);
            foreach (var error in exceptionToHandle.Errors)
            {
                result.ViewData.ModelState.AddModelError(error.Code, error.Message);
            }
                
            context.Result = result;
            
            context.ExceptionHandled = true;
        }
    }
}
