using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.Reader.Domain.Commands.BookComment
{
    public class CreateBookCommentRequest: ICommand<Guid>, IRequest
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        
        public Guid? ParentId { get; set; }
    }
    
    public class CreateBookCommentRequestValidator: AbstractValidator<CreateBookCommentRequest>
    {
        public CreateBookCommentRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.Content).MaximumLength(SharedValidationRules.TextAreaMaxLength);
        }
    }
}