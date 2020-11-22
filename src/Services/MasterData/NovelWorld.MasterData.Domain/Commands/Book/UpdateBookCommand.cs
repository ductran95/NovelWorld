using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class UpdateBookCommand: Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }
    
    public class UpdateBookCommandValidator: AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Summary).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}