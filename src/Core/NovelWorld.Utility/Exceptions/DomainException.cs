using System;

namespace NovelWorld.Utility.Exceptions
{
    public abstract class DomainException: Exception
    {
        public DomainException(string message = "", Exception innerException = null): base(message, innerException)
        {
            
        }
        public abstract HttpException WrapException();
    }
}