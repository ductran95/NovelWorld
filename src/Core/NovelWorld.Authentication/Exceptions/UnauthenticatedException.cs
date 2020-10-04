using System.Net;
using NovelWorld.Common.Exceptions;

namespace NovelWorld.Authentication.Exceptions
{
    public class  UnauthenticatedException: InnerException
    {
        public override HttpException WrapException()
        {
            return new HttpException(Message, HttpStatusCode.Unauthorized, null, this);
        }
    }
}