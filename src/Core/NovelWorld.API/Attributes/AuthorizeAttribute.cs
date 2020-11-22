using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NovelWorld.API.Results;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.API.Attributes
{
    public class AuthorizeAttribute: Attribute, IAuthorizationFilter
    {
        public string Module  { get; set; }
        public string Action { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                
            }
            catch (Exception exception)
            {
                HttpException exceptionToHandle = null;

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

                var response = new Result<bool>
                {
                    Data = false,
                    Success = false,
                    Errors = exceptionToHandle.Errors
                };

                context.Result = new JsonResult(response)
                {
                    StatusCode = (int)exceptionToHandle.StatusCode,
                };
            }
        }
    }
}
