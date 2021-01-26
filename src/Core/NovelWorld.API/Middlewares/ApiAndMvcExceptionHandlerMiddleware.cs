using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelWorld.API.Extensions;
using NovelWorld.API.Results;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;
using NovelWorld.Utility.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Middlewares
{
    public class ApiAndMvcExceptionHandlerMiddleware
    {
        public const string APIPrefix = "/api";
        
        private readonly MvcExceptionHandlerOptions _options;
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiAndMvcExceptionHandlerMiddleware> _logger;

        public ApiAndMvcExceptionHandlerMiddleware(
            RequestDelegate next,
            ILogger<ApiAndMvcExceptionHandlerMiddleware> logger,
            IOptions<MvcExceptionHandlerOptions> options)
        {
            _next = next;
            _logger = logger;
            
            if (options != null && options.Value != null)
            {
                _options = options.Value;
            }
            else
            {
                _options = new MvcExceptionHandlerOptions();
            }
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

                if (context.Request.Path.StartsWithSegments(APIPrefix))
                {
                    var response = new Result<object>()
                    {
                        Data = null,
                        Success = false,
                        Errors = exceptionToHandle.Errors
                    };

                    context.Response.StatusCode = exceptionToHandle.StatusCode;
                    await context.Response.WriteAsJsonAsync(response);
                }
                else
                {
                    try
                    {
                        switch (exceptionToHandle.StatusCode)
                        {
                            case Status401Unauthorized:
                                context.Response.Redirect(_options.UnauthenticatedUrl);
                                break;
                    
                            case Status403Forbidden:
                                context.Response.Redirect(_options.UnauthorizedUrl);
                                break;
                    
                            case Status404NotFound:
                                context.Response.Redirect(_options.NotFoundUrl);
                                break;
                
                            default:
                                var result = new ViewResult {ViewName = _options.ErrorView};
                                result.ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(),
                                    new ModelStateDictionary());
                                result.ViewData.Add("Exception", exceptionToHandle);
                                result.ViewData.Add("Errors", exceptionToHandle.Errors);

                                await context.WriteActionResultAsync(result);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error View not found");
                        await context.Response.WriteAsJsonAsync(exceptionToHandle.Errors);
                    }
                }
            }
        }
    }
}