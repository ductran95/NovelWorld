using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;

namespace NovelWorld.MasterData.Domain.Commands.Category
{
    public class UpdateCategoryRequest: ICommand, IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class UpdateCategoryRequestValidator: AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Description).MaximumLength(CommonValidationRules.TextAreaMaxLength);
        }
    }
}