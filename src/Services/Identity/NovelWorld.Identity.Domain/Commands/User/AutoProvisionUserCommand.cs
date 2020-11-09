using System.Collections.Generic;
using System.Security.Claims;
using FluentValidation;
using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.Commands;

namespace NovelWorld.Identity.Domain.Commands.User
{
    public class AutoProvisionUserCommand: Command<AuthenticatedUser>
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
    }
    
    public class AutoProvisionUserCommandValidator: AbstractValidator<AutoProvisionUserCommand>
    {
        public AutoProvisionUserCommandValidator()
        {
            RuleFor(x => x.Provider).NotEmpty();
            RuleFor(x => x.ProviderUserId).NotEmpty();
            RuleFor(x => x.Claims).NotEmpty();
        }
    }
}