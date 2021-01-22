using System.Collections.Generic;
using System.Security.Claims;
using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.Queries;

namespace NovelWorld.Identity.Domain.Queries.User
{
    public class GetClaimsFromUserQuery: IQuery<IEnumerable<Claim>>
    {
        public AuthenticatedUser User { get; set; }
    }
}