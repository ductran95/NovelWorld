using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.Repositories;

namespace NovelWorld.Infrastructure.EntityFramework.Repositories
{
    public class Repository<T>: IRepository<T> where T: Entity
    {
        protected readonly EntityContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(EntityContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #region Read

        public Task<T> GetById(Guid id, Expression<Func<T, object>> includes = null)
        {
            return GetSingle(x => x.Id == id, includes);
        }

        public Task<T> GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return GetMultiple(condition, includes).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> includes = null)
        {
            var query = _dbSet.AsNoTracking();
            return query;
        }

        public IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return GetAll().Where(condition);
        }

        #endregion

        #region Write

        public int Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public int Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public int Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public int Save(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}