using FluentValidation;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Book
{
    public class SearchBookRequest: SearchRequest<BookGeneralResponse>
    {
    }
    
    public class SearchBookRequestValidator: AbstractValidator<SearchBookRequest>
    {
        public SearchBookRequestValidator()
        {
            
        }
    }
}