using System;
using System.Collections.Generic;
using System.Net;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.Authorization.Exceptions
{
    public class UnauthorizedException: DomainException
    {
        public string Module { get; set; }
        public string Action { get; set; }

        public UnauthorizedException(string module = "", string action = "", string message = "", Exception innerException = null): base(message, innerException)
        {
            Module = module;
            Action = action;
        }
        public override HttpException WrapException()
        {
            var moduleAction = $"{Action} {Module}";
            var errors = new List<Error>
            {
                new Error(CommonErrorCodes.Unauthorized, string.Format(CommonErrorMessages.Unauthorized, moduleAction))
            };
            
            return new HttpException(HttpStatusCode.Forbidden, errors, Message, this);
        }
    }
}