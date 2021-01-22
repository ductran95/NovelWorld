using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Author
{
    public class CreateAuthorRequest: ICommand<Guid>, IRequest
    {
        public string Name { get; set; }
        public string Summary { get; set; }
    }
    
    public class CreateAuthorRequestValidator: AbstractValidator<CreateAuthorRequest>
    {
        public CreateAuthorRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Summary).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}