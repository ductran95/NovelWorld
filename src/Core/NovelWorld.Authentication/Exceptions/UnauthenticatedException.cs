using System.Net;
using NovelWorld.Common.Exceptions;

namespace NovelWorld.Authentication.Exceptions
{
    public class  UnauthenticatedException: DomainException
    {
        public override HttpException WrapException()
        {
            return new HttpException(HttpStatusCode.Unauthorized, null, Message, this);
        }
    }
}