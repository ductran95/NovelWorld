using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Results;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;
using NovelWorld.Utility.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Middlewares
{
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;

        public ApiExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ApiExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
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

                context.Response.StatusCode = exceptionToHandle.StatusCode;
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}