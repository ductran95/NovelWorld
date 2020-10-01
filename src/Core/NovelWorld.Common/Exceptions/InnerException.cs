using System;

namespace NovelWorld.Common.Exceptions
{
    public abstract class InnerException: Exception
    {
        public abstract HttpException WrapException();
    }
}