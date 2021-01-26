using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Results;
using NovelWorld.Utility.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Filters
{
    public class HttpResponseExceptionFilter : IExceptionFilter
    {
        public int Order { get; set; } = 10;
        private readonly ILogger<HttpResponseExceptionFilter> _logger;

        public HttpResponseExceptionFilter(ILogger<HttpResponseExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

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

                exceptionToHandle = new HttpException(Status500InternalServerError, errorResponse, exception.Message, exception);
            }

            _logger.LogError(exceptionToHandle, exceptionToHandle.InnerException != null ? exceptionToHandle.InnerException.Message : exceptionToHandle.Message);

            var response = new Result<object>()
            {
                Data = null,
                Success = false,
                Errors = exceptionToHandle.Errors
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = exceptionToHandle.StatusCode,
            };
            context.ExceptionHandled = true;
        }
    }
}
