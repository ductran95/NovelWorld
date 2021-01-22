using FluentValidation;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Author
{
    public class GetPagingAuthorRequest: PagingRequest<AuthorGeneralResponse>
    {
    }
    
    public class GetPagingAuthorRequestValidator: AbstractValidator<GetPagingAuthorRequest>
    {
        public GetPagingAuthorRequestValidator()
        {
            
        }
    }
}