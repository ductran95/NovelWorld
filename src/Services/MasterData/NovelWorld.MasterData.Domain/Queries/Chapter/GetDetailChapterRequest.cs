using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Chapter
{
    public class GetDetailChapterRequest: IQuery<ChapterDetailResponse>, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class GetDetailChapterRequestValidator: AbstractValidator<GetDetailChapterRequest>
    {
        public GetDetailChapterRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}