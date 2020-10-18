using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FluentValidation.Results;
using NovelWorld.Common;
using NovelWorld.Common.Exceptions;
using NovelWorld.Data.Constants;

namespace NovelWorld.Domain.Exceptions
{
    public class ValidationException: DomainException
    {
        public IEnumerable<ValidationFailure> Errors { get; private set; }

        public ValidationException(IEnumerable<ValidationFailure> errors, string message = "", Exception innerException = null) : base(message, innerException)
        {
            this.Errors = errors;
        }

        public override HttpException WrapException()
        {
            var errors = this.Errors.Select(x => new Error(x.PropertyName, x.ErrorMessage));
            return new HttpException(HttpStatusCode.BadRequest, errors, ErrorCodes.BadRequest, this);
        }
    }
}