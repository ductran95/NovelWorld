using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class ValidateUserCredentialCommandHandler : CommandHandler<ValidateUserCredentialCommand, bool>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        public ValidateUserCredentialCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<ValidateUserCredentialCommandHandler> logger,
            IAuthContext authContext,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository
        ) : base(mediator, mapper, logger, authContext)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public override async Task<bool> Handle(ValidateUserCredentialCommand request,
            CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return false;
            }

            return _passwordHasher.Check(user.Password, request.Password).Verified;
        }
    }
}