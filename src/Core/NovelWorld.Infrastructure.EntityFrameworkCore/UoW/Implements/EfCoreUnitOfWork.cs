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
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Commit();
        }

        public virtual Task CommitAsync()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            return currentTransaction.CommitAsync();
        }

        public virtual void Rollback()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Rollback();
        }

        public virtual Task RollbackAsync()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            return currentTransaction.RollbackAsync();
        }
    }
}