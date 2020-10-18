using System;

namespace NovelWorld.Common.Exceptions
{
    public abstract class DomainException: Exception
    {
        public abstract HttpException WrapException();
    }
}