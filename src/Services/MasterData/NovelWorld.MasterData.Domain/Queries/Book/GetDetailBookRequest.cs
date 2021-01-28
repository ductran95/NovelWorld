using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Book
{
    public class GetDetailBookRequest: IQuery<BookDetailResponse>, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class GetDetailBookRequestValidator: AbstractValidator<GetDetailBookRequest>
    {
        public GetDetailBookRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}