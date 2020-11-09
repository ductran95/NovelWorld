using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using NovelWorld.Identity.Domain.Queries.Abstractions;

namespace NovelWorld.Identity.Domain.Services.Implements
{
    public class ProfileService : IProfileService
    {
        private readonly IUserQuery _userQuery;

        public ProfileService(IUserQuery userQuery)
        {
            _userQuery = userQuery;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var email = subject.FindFirstValue(JwtClaimTypes.Subject);

            var user = await _userQuery.FindByEmail(email);

            if (user == null)
                throw new ArgumentException("Invalid subject identifier");

            var claims = await _userQuery.GetClaimsFromUser(user);
            context.IssuedClaims = claims.ToList();
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

            var email = subject.FindFirstValue(JwtClaimTypes.Subject);
            var user = await _userQuery.FindByEmail(email);

            context.IsActive = user != null;
        }
    }
}
