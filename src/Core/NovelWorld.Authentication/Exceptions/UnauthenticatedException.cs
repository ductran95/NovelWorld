using System.Collections.Generic;
using System.Net;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.Authentication.Exceptions
{
    public class  UnauthenticatedException: DomainException
    {
        
        public override HttpException WrapException()
        {
            var errors = new List<Error>
            {
                new Error(CommonErrorCodes.Unauthenticated, CommonErrorMessages.Unauthenticated)
            };
            
            return new HttpException(HttpStatusCode.Unauthorized, errors, Message, this);
        }
    }
}