using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.DTO;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Identity.Data.Constants;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class AutoProvisionUserCommandHandler : CommandHandler<AutoProvisionUserCommand, AuthenticatedUser>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        
        public AutoProvisionUserCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<AutoProvisionUserCommandHandler> logger,
            IAuthContext authContext,
            IdentityDbContext dbContext,
            IPasswordHasher passwordHasher
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public override async Task<AuthenticatedUser> Handle(AutoProvisionUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = _mapper.Map<Data.Entities.User>(request);
            
            // Set user default password if login via 3rd party
            user.Password = _passwordHasher.Hash(UserConstants.DefaultPassword);
            
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<AuthenticatedUser>(user);
        }
    }
}