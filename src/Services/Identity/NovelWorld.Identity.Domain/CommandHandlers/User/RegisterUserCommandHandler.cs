using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Identity.Data.Constants;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Domain.Queries.User;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class RegisterUserCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IdentityDbContext _dbContext;
        private readonly IPasswordHasher _passwordHasher;
        
        public RegisterUserCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<RegisterUserCommandHandler> logger,
            IAuthContext authContext,
            IdentityDbContext dbContext,
            IPasswordHasher passwordHasher
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
        }

        public override async Task<bool> Handle(RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var existedUser = await _mediator.Send(new GetUserByEmailQuery()
            {
                Email = request.Email
            });

            if (existedUser != null)
            {
                throw new DuplicateDataException(ErrorCodes.UserHasExist,
                    string.Format(ErrorMessages.UserHasExist, request.Email));
            }
            
            var user = _mapper.Map<Data.Entities.User>(request);
            user.Password = _passwordHasher.Hash(request.Password);
            
            await _dbContext.Users.AddAsync(user, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}