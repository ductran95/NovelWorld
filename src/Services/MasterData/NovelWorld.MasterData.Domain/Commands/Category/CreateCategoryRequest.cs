using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;
using NovelWorld.Shared.Data.Constants;

namespace NovelWorld.MasterData.Domain.Commands.Category
{
    public class CreateCategoryRequest: ICommand<Guid>, IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class CreateCategoryRequestValidator: AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(SharedValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Description).MaximumLength(SharedValidationRules.TextAreaMaxLength);
        }
    }
}