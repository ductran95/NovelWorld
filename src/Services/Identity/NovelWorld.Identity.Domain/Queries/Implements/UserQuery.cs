using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Authentication.DTO;
using NovelWorld.Common.Helpers.Abstractions;
using NovelWorld.Domain.Queries.Implements;
using NovelWorld.Identity.Domain.Queries.Abstractions;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;

namespace NovelWorld.Identity.Domain.Queries.Implements
{
    public class UserQuery : Query, IUserQuery
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        public UserQuery(
            IMapper mapper,
            ILogger<Query> logger,
            IAuthContext authContext,
            IPasswordHasher passwordHasher,
            IUserRepository userRepository
        ) : base(mapper, logger, authContext)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
        }

        public async Task<bool> ValidateCredentials(string email, string password)
        {
            var user =  await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            return _passwordHasher.Check(user.Password, password).Verified;
        }

        public async Task<AuthenticatedUser> FindByEmail(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return _mapper.Map<AuthenticatedUser>(user);
        }

        public async Task<AuthenticatedUser> FindByExternalProvider(string provider, string providerUserId)
        {
            var user = await _userRepository.GetByEmailAsync(providerUserId);
            return _mapper.Map<AuthenticatedUser>(user);
        }
    }
}