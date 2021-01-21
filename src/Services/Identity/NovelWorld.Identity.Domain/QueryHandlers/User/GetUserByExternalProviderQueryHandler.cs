using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Identity.Domain.Queries.User;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.QueryHandlers.User
{
    public class GetUserByExternalProviderQueryHandler : QueryHandler<GetUserByExternalProviderQuery, AuthenticatedUser>
    {
        private readonly IUserRepository _userRepository;
        public GetUserByExternalProviderQueryHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<GetUserByExternalProviderQueryHandler> logger,
            IAuthContext authContext,
            IUserRepository userRepository
        ) : base(mediator, mapper, logger, authContext)
        {
            _userRepository = userRepository;
        }

        public override async Task<AuthenticatedUser> Handle(GetUserByExternalProviderQuery request,
            CancellationToken cancellationToken)
        {
            var user =  await _userRepository.GetByEmailAsync(request.ProviderUserId);
            return _mapper.Map<AuthenticatedUser>(user);
        }
    }
}