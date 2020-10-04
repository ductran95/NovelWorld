using System.Data;
using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.UoW
{
    public interface IUnitOfWork
    {
        bool IsInTransaction();
        void BeginTransaction();
        Task BeginTransactionAsync();
        void Commit();
        Task CommitAsync();
        void Rollback();
        Task RollbackAsync();
    }
}