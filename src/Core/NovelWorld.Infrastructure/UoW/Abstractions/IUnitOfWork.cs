using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.UoW.Abstractions
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