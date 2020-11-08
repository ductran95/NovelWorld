using System;
using System.Collections.Generic;
using System.Net;
using NovelWorld.Common;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.DTO;

namespace NovelWorld.Domain.Exceptions
{
    public class DuplicateDataException: DomainException
    {
        public string Code { get; private set; }

        public DuplicateDataException(string code, string message, Exception innerException = null) : base(message, innerException)
        {
            this.Code = code;
        }

        public override HttpException WrapException()
        {
            var errors = new List<Error>
            {
                new Error(Code, Message)
            };
            return new HttpException(HttpStatusCode.NotFound, errors, Message, this);
        }
    }
}