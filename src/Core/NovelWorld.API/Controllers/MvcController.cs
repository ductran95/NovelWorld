using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;
using NovelWorld.Mediator;
using NovelWorld.Utility.Exceptions;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Controllers
{
    public abstract class MvcController: Controller
    {
        protected readonly IWebHostEnvironment _environment;
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<MvcController> _logger;
        protected readonly IAuthContext _authContext;
        
        public MvcController(
            IWebHostEnvironment environment,
            IMediator mediator, 
            IMapper mapper, 
            ILogger<MvcController> logger, 
            IAuthContext authContext
        )
        {
            _environment = environment;
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }

        protected void HandleException(Exception exception)
        {
            if (_environment.IsDevelopment())
            {
                ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(exception);
                exInfo.Throw();
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

                exceptionToHandle = new HttpException(Status500InternalServerError, errorResponse, exception.Message, exception);
            }

            // Will delegate to default Exception filter
            // when exception is 401, 403, 404
            if (exceptionToHandle.StatusCode == Status401Unauthorized ||
                exceptionToHandle.StatusCode == Status403Forbidden || exceptionToHandle.StatusCode == Status404NotFound)
            {
                ExceptionDispatchInfo exInfo = ExceptionDispatchInfo.Capture(exception);
                exInfo.Throw();
            }
            else
            {
                _logger.LogError(exceptionToHandle, exceptionToHandle.InnerException != null ? exceptionToHandle.InnerException.Message : exceptionToHandle.Message);
            
                HttpContext.Items.TryAdd("Exception", exceptionToHandle);
                HttpContext.Items.TryAdd("Errors", exceptionToHandle.Errors);
            }
        }
    }
}