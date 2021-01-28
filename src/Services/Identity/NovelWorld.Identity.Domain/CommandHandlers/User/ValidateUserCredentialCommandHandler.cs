using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class ValidateUserCredentialCommandHandler : CommandHandler<ValidateUserCredentialCommand, bool>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        
        public ValidateUserCredentialCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<ValidateUserCredentialCommandHandler> logger,
            IAuthContext authContext,
            IdentityDbContext dbContext,
            IPasswordHasher passwordHasher
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public override async Task<bool> Handle(ValidateUserCredentialCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken: cancellationToken);
            
            if (user == null)
            {
                return false;
            }

            return _passwordHasher.Check(user.Password, request.Password).Verified;
        }
    }
}