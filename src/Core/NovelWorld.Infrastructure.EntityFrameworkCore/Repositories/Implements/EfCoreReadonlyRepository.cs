using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements
{
    public class EfCoreReadonlyRepository<TContext, T>: IEfCoreReadonlyRepository<T> where TContext: EfCoreEntityDbContext where T: Entity
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dbSet;
        public EfCoreReadonlyRepository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T GetById(Guid id, Expression<Func<T, object>> includes = null, bool track = false)
        {
            return GetSingle(x => x.Id == id, includes, track);
        }

        public Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>> includes = null, bool track = false,
            CancellationToken cancellationToken = default)
        {
            return GetSingleAsync(x => x.Id == id, includes, track, cancellationToken);
        }

        public T GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null,
            bool track = false)
        {
            return GetAll(includes, track).Where(condition).FirstOrDefault();
        }

        public Task<T> GetSingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null,
            bool track = false, CancellationToken cancellationToken = default)
        {
            return GetAll(includes, track).Where(condition).FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> includes = null, bool track = false)
        {
            var query = _dbSet.AsNoTracking();

            if (track)
            {
                query = _dbSet.AsQueryable();
            }

            if (includes != null)
            {
                query = query.Includes(includes);
            }

            return query;
        }

        public IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition,
            Expression<Func<T, object>> includes = null, bool track = false)
        {
            return GetAll(includes, track).Where(condition);
        }

        public IQueryable<T> GetAllIncludeDeleted(Expression<Func<T, object>> includes = null, bool track = false)
        {
            var query = _dbSet.AsNoTracking();

            if (track)
            {
                query = _dbSet.AsQueryable();
            }

            if (includes != null)
            {
                query = query.Includes(includes);
            }

            return query.IgnoreQueryFilters();
        }
    }
}