using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Authentication.DTO;
using NovelWorld.Common.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Identity.Data.Constants;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class AutoProvisionUserCommandHandler : CommandHandler<AutoProvisionUserCommand, AuthenticatedUser>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        public AutoProvisionUserCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<CommandHandler<AutoProvisionUserCommand, AuthenticatedUser>> logger,
            IAuthContext authContext,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository
        ) : base(mediator, mapper, logger, authContext)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public override async Task<AuthenticatedUser> Handle(AutoProvisionUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Data.Entities.User>(request);
            
            // Set user default password if login via 3rd party
            user.Password = _passwordHasher.Hash(UserConstants.DefaultPassword);
            
            await _userRepository.AddAsync(user);
            return _mapper.Map<AuthenticatedUser>(user);
        }
    }
}