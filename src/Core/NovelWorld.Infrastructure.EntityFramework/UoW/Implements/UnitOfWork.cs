using System.Threading.Tasks;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.UoW.Abstractions;

namespace NovelWorld.Infrastructure.EntityFramework.UoW.Implements
{
    public class UnitOfWork: IUnitOfWork
    {
        protected readonly EntityContext _context;
        
        public UnitOfWork(EntityContext context)
        {
            _context = context;
        }
        
        public bool IsInTransaction()
        {
            return _context.Database.CurrentTransaction != null;
        }

        public void BeginTransaction()
        {
            _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Commit();
        }

        public Task CommitAsync()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            return currentTransaction.CommitAsync();
        }

        public void Rollback()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            currentTransaction.Rollback();
        }

        public Task RollbackAsync()
        {
            var currentTransaction = _context.Database.CurrentTransaction;
            return currentTransaction.RollbackAsync();
        }
    }
}