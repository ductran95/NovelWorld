﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelWorld.Utility.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Filters
{
    public class MvcExceptionHandlerFilter : IExceptionFilter
    {
        private readonly ILogger<MvcExceptionHandlerFilter> _logger;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly MvcExceptionHandlerOptions _options;

        public MvcExceptionHandlerFilter(
            ILogger<MvcExceptionHandlerFilter> logger,
            IModelMetadataProvider modelMetadataProvider,
            IOptions<MvcExceptionHandlerOptions> options
            )
        {
            _logger = logger;
            _modelMetadataProvider = modelMetadataProvider;
            
            if (options != null && options.Value != null)
            {
                _options = options.Value;
            }
            else
            {
                _options = new MvcExceptionHandlerOptions();
            }
        }

        public void OnException(ExceptionContext context)
        {
            OnExceptionInternal(context, _logger, _options, _modelMetadataProvider);
        }
        
        internal static void OnExceptionInternal(ExceptionContext context, ILogger logger, MvcExceptionHandlerOptions options, IModelMetadataProvider modelMetadataProvider)
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

            logger.LogError(exceptionToHandle, exceptionToHandle.InnerException != null ? exceptionToHandle.InnerException.Message : exceptionToHandle.Message);
            
            IActionResult result;

            try
            {
                switch (exceptionToHandle.StatusCode)
                {
                    case Status401Unauthorized:
                        result = new RedirectResult(options.UnauthenticatedUrl);
                        break;

                    case Status403Forbidden:
                        result = new RedirectResult(options.UnauthorizedUrl);
                        break;

                    case Status404NotFound:
                        result = new RedirectResult(options.NotFoundUrl);
                        break;

                    default:
                        result = new ViewResult {ViewName = options.ErrorView};

                        var viewResult = (ViewResult) result;
                        viewResult.ViewData = new ViewDataDictionary(modelMetadataProvider,
                            context.ModelState);
                        viewResult.ViewData.Add("Exception", exceptionToHandle);
                        viewResult.ViewData.Add("Errors", exceptionToHandle.Errors);
                        break;
                }
                
                context.Result = result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error View not found");
                context.Result = new JsonResult(exceptionToHandle.Errors)
                {
                    StatusCode = exceptionToHandle.StatusCode,
                };
            }
            
            context.ExceptionHandled = true;
        }
    }
}
