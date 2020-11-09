using System.Threading.Tasks;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements;

namespace NovelWorld.Identity.Infrastructure.Repositories.Implements
{
    public class UserRepository : EfCoreRepository<IdentityContext, User>, IUserRepository
    {
        public UserRepository(
            IdentityContext context
            ) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return GetSingle(x => x.Email == email);
        }

        public Task<User> GetByEmailAsync(string email)
        {
            return GetSingleAsync(x => x.Email == email);
        }
    }
}