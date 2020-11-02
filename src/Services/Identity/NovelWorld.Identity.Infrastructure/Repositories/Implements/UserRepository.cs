using System.Threading.Tasks;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Identity.Infrastructure.Repositories.Abstracts;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.EntityFramework.Repositories.Implements;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Identity.Infrastructure.Repositories.Implements
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(
            IdentityContext context, 
            IReadonlyRepository<User> readonlyRepository,
            IWriteonlyRepository<User> writeonlyRepository
            ) : base(context, readonlyRepository, writeonlyRepository)
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