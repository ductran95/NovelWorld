using System.Net;
using NovelWorld.Common.Exceptions;

namespace NovelWorld.Authorization.Exceptions
{
    public class UnauthorizedException: InnerException
    {
        public override HttpException WrapException()
        {
            return new HttpException(Message, HttpStatusCode.Forbidden, null, this);
        }
    }
}