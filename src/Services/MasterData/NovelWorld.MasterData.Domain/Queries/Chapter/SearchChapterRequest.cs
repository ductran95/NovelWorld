using FluentValidation;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Chapter
{
    public class SearchChapterRequest: SearchRequest<ChapterGeneralResponse>
    {
    }
    
    public class SearchChapterRequestValidator: AbstractValidator<SearchChapterRequest>
    {
        public SearchChapterRequestValidator()
        {
            
        }
    }
}