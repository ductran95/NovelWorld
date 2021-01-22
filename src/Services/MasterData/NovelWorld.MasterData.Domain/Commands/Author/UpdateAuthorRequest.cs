using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Author
{
    public class UpdateAuthorRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
    }
    
    public class UpdateAuthorRequestValidator: AbstractValidator<UpdateAuthorRequest>
    {
        public UpdateAuthorRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Summary).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}