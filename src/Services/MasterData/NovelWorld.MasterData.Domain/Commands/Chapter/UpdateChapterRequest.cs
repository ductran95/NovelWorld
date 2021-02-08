using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.MasterData.Domain.Commands.Chapter
{
    public class UpdateChapterRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
    }
    
    public class UpdateChapterRequestValidator: AbstractValidator<UpdateChapterRequest>
    {
        public UpdateChapterRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Number).GreaterThan(0).When(x => x.Number != null);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(SharedValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}