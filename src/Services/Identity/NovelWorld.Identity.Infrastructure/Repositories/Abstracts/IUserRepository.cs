using System.Threading.Tasks;
using NovelWorld.Identity.Data.Entities;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Identity.Infrastructure.Repositories.Abstracts
{
    public interface IUserRepository: IRepository<User>
    {
        User GetByEmail(string email);
        Task<User> GetByEmailAsync(string email);
    }
}