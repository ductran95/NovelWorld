using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.EntityFramework.Extensions;
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
            return GetSingle(x => x.Id == id, includes);
        }

        public Task<T> GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return GetAll(includes).Where(condition).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> includes = null)
        {
            var result = _dbSet.AsNoTracking();
            if (includes != null)
            {
                result = result.Includes(includes);
            }

            return result;
        }

        public IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return GetAll(includes).Where(condition);
        }
    }
}