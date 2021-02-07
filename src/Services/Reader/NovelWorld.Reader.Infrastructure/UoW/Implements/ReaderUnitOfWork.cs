using NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.Reader.Infrastructure.Contexts;

namespace NovelWorld.Reader.Infrastructure.UoW.Implements
{
    public class ReaderUnitOfWork: EfCoreUnitOfWork<ReaderDbContext>, IUnitOfWork
    {
        public ReaderUnitOfWork(ReaderDbContext dbContext): base(dbContext)
        {
        }
    }
}