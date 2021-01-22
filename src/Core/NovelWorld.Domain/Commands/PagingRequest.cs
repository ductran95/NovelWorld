using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Validators;

namespace NovelWorld.Domain.Commands
{
    public class PagingRequest<T>: PagingRequest, ICommand<T>
    {
    }
    
    public class PagingRequestValidator<T>: AbstractValidator<PagingRequest<T>>
    {
        public PagingRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleForEach(x => x.Filters).ValidateField();
            RuleForEach(x => x.Sorts).ValidateField();
        }
    }
}