using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Author
{
    public class GetDetailAuthorRequest: IQuery<AuthorDetailResponse>, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class GetDetailAuthorRequestValidator: AbstractValidator<GetDetailAuthorRequest>
    {
        public GetDetailAuthorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}