using System.Collections.Generic;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Chapter;

namespace NovelWorld.MasterData.Domain.Queries.Chapter
{
    public class GetAllChapterRequest: IQuery<List<ChapterGeneralResponse>>, IRequest
    {
    }
    
    public class GetAllChapterRequestValidator: AbstractValidator<GetAllChapterRequest>
    {
        public GetAllChapterRequestValidator()
        {
        }
    }
}