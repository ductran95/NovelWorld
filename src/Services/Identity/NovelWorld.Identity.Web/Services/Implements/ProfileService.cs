using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using NovelWorld.Identity.Domain.Queries.User;
using NovelWorld.Mediator;

namespace NovelWorld.Identity.Web.Services.Implements
{
    public class ProfileService : IProfileService
    {
        private readonly IMediator _mediator;

        public ProfileService(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var email = subject.FindFirstValue(JwtClaimTypes.Subject);
            var user = await _mediator.Send(new GetAuthenticatedUserByEmailQuery()
            {
                Email = email
            });

            if (user == null)
                throw new ArgumentException("Invalid subject identifier");

            var claims = await _mediator.Send(new GetClaimsFromUserQuery()
            {
                User = user
            });
            context.IssuedClaims = claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var email = subject.FindFirstValue(JwtClaimTypes.Subject);
            var user = await _mediator.Send(new GetAuthenticatedUserByEmailQuery()
            {
                Email = email
            });

            context.IsActive = user != null;
        }
    }
}
