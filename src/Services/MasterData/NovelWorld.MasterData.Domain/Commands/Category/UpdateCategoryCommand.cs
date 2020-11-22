using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands
{
    public class UpdateCategoryCommand: Command
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }
    
    public class UpdateCategoryCommandValidator: AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Summary).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}