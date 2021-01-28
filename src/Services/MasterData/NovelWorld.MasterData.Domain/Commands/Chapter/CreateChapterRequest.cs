using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Chapter
{
    public class CreateChapterRequest: ICommand<Guid>, IRequest
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
    }
    
    public class CreateChapterRequestValidator: AbstractValidator<CreateChapterRequest>
    {
        public CreateChapterRequestValidator()
        {
            RuleFor(x => x.Number).GreaterThan(0).When(x => x.Number != null);
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.BookId).NotEmpty();
        }
    }
}