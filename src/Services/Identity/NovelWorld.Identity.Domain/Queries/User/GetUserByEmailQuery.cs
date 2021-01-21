using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.Queries;

namespace NovelWorld.Identity.Domain.Queries.User
{
    public class GetUserByEmailQuery: Query<AuthenticatedUser>
    {
        public string Email { get; set; }
    }
}