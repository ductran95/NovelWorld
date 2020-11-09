using System;
using FluentValidation;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Commands;

namespace NovelWorld.Identity.Domain.Commands.User
{
    public class RegisterUserCommand: Command
    {
        public string Account { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DoB { get; set; }
        public string Password { get; set; }
    }
    
    public class RegisterUserCommandValidator: AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Account).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Email).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength).EmailAddress();
            RuleFor(x => x.PhoneNumber).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.Address).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
            RuleFor(x => x.DoB).LessThan(DateTime.Today);
            RuleFor(x => x.Password).NotEmpty().MaximumLength(CommonValidationRules.TextFieldMaxLength);
        }
    }
}