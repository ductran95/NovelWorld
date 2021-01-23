using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Data.Requests;
using NovelWorld.Domain.Commands;
using NovelWorld.MasterData.Data.Enums;

namespace NovelWorld.MasterData.Domain.Commands.Book
{
    public class CreateBookRequest: ICommand<Guid>, IRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BookStatusEnum Status { get; set; }
        public float Rate { get; set; }
        public Guid AuthorId { get; set; }
    }
    
    public class CreateBookRequestValidator: AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Description).MaximumLength(CommonValidationRules.TextAreaMaxLength);
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.Rate).GreaterThanOrEqualTo(0).LessThanOrEqualTo(10);
            RuleFor(x => x.AuthorId).NotEmpty();
        }
    }
}