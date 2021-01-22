using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;
using NovelWorld.Data.Constants;
using NovelWorld.Utility.Exceptions;
using NovelWorld.Data.DTO;

namespace NovelWorld.Domain.Exceptions
{
    public class BadRequestException: DomainException
    {
        public IEnumerable<ValidationFailure> Errors { get; private set; }

        public BadRequestException(IEnumerable<ValidationFailure> errors = null, string message = "", Exception innerException = null) : base(message, innerException)
        {
            Errors = errors;
        }

        public override HttpException WrapException()
        {
            var errors = new List<Error>();

            if (Errors != null && Errors.Any())
            {
                errors = Errors.Select(x => new Error(x.PropertyName, x.ErrorMessage)).ToList();
            }
            else if(!string.IsNullOrWhiteSpace(Message))
            {
                errors.Add(new Error(CommonErrorCodes.BadRequest, Message));
            }
            
            return new HttpException(400, errors, Message, this);
        }
    }
}