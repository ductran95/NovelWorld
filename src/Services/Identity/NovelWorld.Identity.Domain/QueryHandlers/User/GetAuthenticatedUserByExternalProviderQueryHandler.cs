using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Identity.Domain.Queries.User;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.QueryHandlers.User
{
    public class GetAuthenticatedUserByExternalProviderQueryHandler : QueryHandler<GetAuthenticatedUserByExternalProviderQuery, AuthenticatedUser>
    {
        private readonly IdentityDbContext _dbContext;
        public GetAuthenticatedUserByExternalProviderQueryHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<GetAuthenticatedUserByExternalProviderQueryHandler> logger,
            IAuthContext authContext,
            IdentityDbContext dbContext
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<AuthenticatedUser> Handle(GetAuthenticatedUserByExternalProviderQuery request,
            CancellationToken cancellationToken)
        {
            var user =  await _dbContext.Users.FirstOrDefaultAsync(x=>x.Email == request.ProviderUserId, cancellationToken: cancellationToken);
            return _mapper.Map<AuthenticatedUser>(user);
        }
    }
}