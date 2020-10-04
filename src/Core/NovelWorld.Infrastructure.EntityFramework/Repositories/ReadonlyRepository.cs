using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.Repositories;

namespace NovelWorld.Infrastructure.EntityFramework.Repositories
{
    public class ReadonlyRepository<T>: IReadonlyRepository<T> where T: Entity
    {
        protected readonly EntityContext _context;
        protected readonly DbSet<T> _dbSet;
        public ReadonlyRepository(EntityContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        
        public Task<T> GetById(Guid id, Expression<Func<T, object>> includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> includes = null)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            throw new NotImplementedException();
        }
    }
}