using System.Threading;
using System.Threading.Tasks;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.UoW.Implements
{
    public class EfCoreUnitOfWork<TContext>: IUnitOfWork where TContext: EfCoreEntityContext
    {
        protected readonly TContext _context;
        
        public EfCoreUnitOfWork(TContext context)
        {
            _context = context;
        }
        
        public virtual bool IsInTransaction()
        {
            return _context.Database.CurrentTransaction != null;
        }

        public virtual void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public virtual async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public virtual void Commit()
        {
            _context.SaveChanges();
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Commit();
        }

        public virtual async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
            var currentTransaction = _context.Database.CurrentTransaction;
            await currentTransaction.CommitAsync(cancellationToken);
        }

        public virtual void Rollback()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Rollback();
        }

        public virtual async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            await currentTransaction.RollbackAsync(cancellationToken);
        }
    }
}