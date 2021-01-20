using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using NovelWorld.Utility.Extensions;
using NovelWorld.Mediator.Pipeline;

namespace NovelWorld.Domain.Proxies
{
    public class ValidationProxy<T> : IRequestPreProcessor<T>, INotificationPreProcessor<T>
    {
        #region Properties
        protected readonly IEnumerable<IValidator<T>> _validators;
        #endregion

        #region Constructor
        public ValidationProxy(IEnumerable<IValidator<T>> validators)
        {
            this._validators = validators;
        }
        #endregion

        public async Task Process(T request, CancellationToken cancellationToken)
        {
            var requestName = request.GetGenericTypeName();
            // Check request is Command or Event
            // If request is an Event, then do not thow Exception on validation failed
            var type = request is INotification ? "event" : "command";
            var shoudldThrowException = type == "command";
            try
            {
                if (_validators.Any())
                {
                    var errors = await _validators.Select(async v => await v.ValidateAsync(request)).SelectManyAsync(x => x.Errors);

                    if (errors.Any())
                    {
                        throw new NovelWorld.Domain.Exceptions.ValidationException(errors, $"Validation Errors for {type} {requestName}");
                    }
                }
            }
            catch (Exception)
            {
                if (shoudldThrowException)
                {
                    throw;
                }
            }
        }
    }
}
