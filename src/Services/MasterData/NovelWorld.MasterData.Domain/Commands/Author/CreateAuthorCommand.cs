using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class CreateAuthorCommand: Command<Guid>
    {
        public string Name { get; set; }
        public string Summary { get; set; }
    }
    
    public class CreateAuthorCommandValidator: AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Summary).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}