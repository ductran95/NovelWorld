using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Authentication.DTO;
using NovelWorld.Utility.Helpers.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Identity.Data.Constants;
using NovelWorld.Identity.Domain.Commands.User;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.CommandHandlers.User
{
    public class RegisterUserCommandHandler : CommandHandler<RegisterUserCommand>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        public RegisterUserCommandHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<CommandHandler<RegisterUserCommand>> logger,
            IAuthContext authContext,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository
        ) : base(mediator, mapper, logger, authContext)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public override async Task<bool> Handle(RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var checkUserExist = await _userRepository.GetByEmailAsync(request.Email);

            if (checkUserExist != null)
            {
                throw new DuplicateDataException(ErrorCodes.UserHasExist,
                    string.Format(ErrorMessages.UserHasExist, request.Email));
            }
            
            var user = _mapper.Map<Data.Entities.User>(request);
            user.Password = _passwordHasher.Hash(request.Password);
            
            await _userRepository.AddAsync(user);

            return true;
        }
    }
}