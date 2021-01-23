using FluentValidation;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Author
{
    public class SearchAuthorRequest: SearchRequest<AuthorGeneralResponse>
    {
    }
    
    public class SearchAuthorRequestValidator: AbstractValidator<SearchAuthorRequest>
    {
        public SearchAuthorRequestValidator()
        {
            
        }
    }
}