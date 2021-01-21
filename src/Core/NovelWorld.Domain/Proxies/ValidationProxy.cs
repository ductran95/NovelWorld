using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR.Pipeline;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Utility.Extensions;

namespace NovelWorld.Domain.Proxies
{
    public sealed class ValidationProxy<T> : IRequestPreProcessor<T>
    {
        #region Properties
        private readonly IEnumerable<IValidator<T>> _validators;
        #endregion

        #region Constructor
        public ValidationProxy(IEnumerable<IValidator<T>> validators)
        {
            _validators = validators;
        }
        #endregion

        public async Task Process(T request, CancellationToken cancellationToken)
        {
            var requestName = request.GetGenericTypeName();

            if (_validators.Any())
            {
                var errors = await _validators.Select(async v => await v.ValidateAsync(request)).SelectManyAsync(x => x.Errors);

                if (errors.Any())
                {
                    throw new ValidateException(errors, $"Validation Errors for {requestName}");
                }
            }
        }
    }
}
