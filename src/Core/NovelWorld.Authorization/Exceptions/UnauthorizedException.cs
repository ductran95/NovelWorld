using System.Net;
using NovelWorld.Common.Exceptions;

namespace NovelWorld.Authorization.Exceptions
{
    public class UnauthorizedException: DomainException
    {
        public override HttpException WrapException()
        {
            return new HttpException(HttpStatusCode.Forbidden, null, Message, this);
        }
    }
}