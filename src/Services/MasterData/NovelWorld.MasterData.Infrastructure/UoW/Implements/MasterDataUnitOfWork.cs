using NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements;
using NovelWorld.Infrastructure.UoW.Abstractions;
using NovelWorld.MasterData.Infrastructure.Contexts;

namespace NovelWorld.MasterData.Infrastructure.UoW.Implements
{
    public class MasterDataUnitOfWork: EfCoreUnitOfWork<MasterDataContext>, IUnitOfWork
    {
        public MasterDataUnitOfWork(MasterDataContext context): base(context)
        {
        }
    }
}