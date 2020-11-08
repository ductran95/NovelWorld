using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Results;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

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

            var response = new Result<bool>()
            {
                Data = false,
                Success = false,
                Errors = exceptionToHandle.Errors
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = (int)exceptionToHandle.StatusCode,
            };
            context.ExceptionHandled = true;
        }
    }
}
