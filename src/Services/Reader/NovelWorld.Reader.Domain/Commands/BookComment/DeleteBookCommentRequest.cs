using System;
using FluentValidation;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.Reader.Domain.Commands.BookComment
{
    public class DeleteBookCommentRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
    }
    
    public class DeleteBookCommentRequestValidator: AbstractValidator<DeleteBookCommentRequest>
    {
        public DeleteBookCommentRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}