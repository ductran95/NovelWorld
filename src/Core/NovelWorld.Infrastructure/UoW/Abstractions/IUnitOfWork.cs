using System.Threading;
using System.Threading.Tasks;

namespace NovelWorld.Infrastructure.UoW.Abstractions
{
    public interface IUnitOfWork
    {
        bool IsInTransaction();
        void BeginTransaction();
        Task BeginTransactionAsync();
        void Commit();
        Task CommitAsync(CancellationToken cancellationToken = default);
        void Rollback();
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}