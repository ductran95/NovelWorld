using System;
using System.Collections.Generic;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Queries;
using NovelWorld.Reader.Data.Responses.BookComment;

namespace NovelWorld.Reader.Domain.Queries.BookComment
{
    public class GetBookCommentsTreeViewRequest: IQuery<List<BookCommentTreeViewResponse>>, IRequest
    {
        public Guid BookId { get; set; }
    }
    
    public class GetBookCommentsTreeViewRequestValidator: AbstractValidator<BookCommentTreeViewResponse>
    {
        public GetBookCommentsTreeViewRequestValidator()
        {
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}