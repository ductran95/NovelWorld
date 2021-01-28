using FluentValidation;
using NovelWorld.Data.DTO;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Validators;

namespace NovelWorld.Domain.Queries
{
    public class SearchRequest<T>: SearchRequest, IQuery<PagedData<T>>
    {
    }
    
    public class SearchRequestValidator<T>: AbstractValidator<SearchRequest<T>>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleForEach(x => x.Filters).ValidateField();
            RuleForEach(x => x.Sorts).ValidateField();
        }
    }
}