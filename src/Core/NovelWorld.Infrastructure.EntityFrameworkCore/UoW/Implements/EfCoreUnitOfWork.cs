using System.Threading;
using System.Threading.Tasks;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements
{
    public class EfCoreUnitOfWork<TContext>: IUnitOfWork where TContext: EfCoreEntityDbContext
    {
        protected readonly TContext _dbContext;
        
        public EfCoreUnitOfWork(TContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public virtual bool IsInTransaction()
        {
            return _dbContext.Database.CurrentTransaction != null;
        }

        public virtual void BeginTransaction()
        {
            _dbContext.Database.BeginTransaction();
        }

        public virtual async Task BeginTransactionAsync()
        {
            await _dbContext.Database.BeginTransactionAsync();
        }

        public virtual void Commit()
        {
            _dbContext.SaveChanges();
            var currentTransaction = _dbContext.Database.CurrentTransaction;
            currentTransaction.Commit();
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            var currentTransaction = _dbContext.Database.CurrentTransaction;
            await currentTransaction.CommitAsync(cancellationToken);
        }

        public virtual void Rollback()
        {
            var currentTransaction = _dbContext.Database.CurrentTransaction;
            currentTransaction.Rollback();
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            var currentTransaction = _dbContext.Database.CurrentTransaction;
            await currentTransaction.RollbackAsync(cancellationToken);
        }
    }
}