using NovelWorld.Identity.Infrastructure.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Identity.Infrastructure.UoW.Implements
{
    public class IdentityUnitOfWork: EfCoreUnitOfWork<IdentityDbContext>, IUnitOfWork
    {
        public IdentityUnitOfWork(IdentityDbContext dbContext): base(dbContext)
        {
        }
    }
}