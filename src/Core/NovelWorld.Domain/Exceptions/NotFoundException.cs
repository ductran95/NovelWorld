using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;
using NovelWorld.Common;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;
using NovelWorld.Data.DTO;

namespace NovelWorld.Domain.Exceptions
{
    public class NotFoundException: DomainException
    {
        public object DataId { get; private set; }

        public NotFoundException(object dataId, string message = "", Exception innerException = null) : base(message, innerException)
        {
            this.DataId = dataId;
        }

        public override HttpException WrapException()
        {
            var errors = new List<Error>
            {
                new Error(CommonErrorCodes.NotFound, string.Format(CommonErrorMessages.DataNotFound, DataId))
            };
            return new HttpException(HttpStatusCode.NotFound, errors, Message, this);
        }
    }
}