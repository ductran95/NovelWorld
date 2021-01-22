using NovelWorld.Authentication.DTO;
using NovelWorld.Domain.Queries;

namespace NovelWorld.Identity.Domain.Queries.User
{
    public class GetUserByExternalProviderQuery: IQuery<AuthenticatedUser>
    {
        public string Provider { get; set; }
        public string ProviderUserId { get; set; }
    }
}