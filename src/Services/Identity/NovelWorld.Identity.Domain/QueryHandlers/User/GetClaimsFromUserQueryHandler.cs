using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityModel;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Identity.Domain.Queries.User;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Domain.QueryHandlers.User
{
    public class GetClaimsFromUserQueryHandler : QueryHandler<GetClaimsFromUserQuery, IEnumerable<Claim>>
    {
        private readonly IUserRepository _userRepository;
        public GetClaimsFromUserQueryHandler(
            IMediator mediator,
            IMapper mapper,
            ILogger<GetClaimsFromUserQueryHandler> logger,
            IAuthContext authContext,
            IUserRepository userRepository
        ) : base(mediator, mapper, logger, authContext)
        {
            _userRepository = userRepository;
        }

        public override async Task<IEnumerable<Claim>> Handle(GetClaimsFromUserQuery request,
            CancellationToken cancellationToken)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtClaimTypes.Subject, request.User.Email),
                new Claim(JwtClaimTypes.Email, request.User.Email),
                new Claim(AdditionalClaimTypes.UserId, request.User.Id.ToString()),
                new Claim(AdditionalClaimTypes.Account, request.User.Account),
                new Claim(AdditionalClaimTypes.UserFullName, request.User.FullName),
                new Claim(AdditionalClaimTypes.UserEmail, request.User.Email),
            };

            // TODO: get user's roles
            return await Task.FromResult(claims);
        }
    }
}