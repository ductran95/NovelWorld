using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.Reader.Domain.Commands.BookComment
{
    public class UpdateBookCommentRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        
        public Guid? ParentId { get; set; }
    }
    
    public class UpdateBookCommentRequestValidator: AbstractValidator<UpdateBookCommentRequest>
    {
        public UpdateBookCommentRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
            RuleFor(x => x.Content).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}