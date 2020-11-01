using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFramework.Repositories.Implements
{
    public class Repository<T>: IRepository<T> where T: Entity
    {
        protected IReadonlyRepository<T> _readonlyRepository;
        protected IWriteonlyRepository<T> _writeonlyRepository;
        protected readonly EntityContext _context;
        protected readonly DbSet<T> _dbSet;
        public Repository(EntityContext context, IReadonlyRepository<T> readonlyRepository, IWriteonlyRepository<T> writeonlyRepository)
        {
            _context = context;
            _dbSet = _context.Set<T>();

            _readonlyRepository = readonlyRepository;
            _writeonlyRepository = writeonlyRepository;
        }

        #region Read

        public T GetById(Guid id, Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetById(id, includes);
        }
        
        public Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetByIdAsync(id, includes);
        }

        public T GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetSingle(condition, includes);
        }
        
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetSingleAsync(condition, includes);
        }

        public IQueryable<T> GetAll(Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetAll(includes);
        }

        public IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return _readonlyRepository.GetMultiple(condition, includes);
        }

        #endregion

        #region Write

        public int Add(T entity)
        {
            return _writeonlyRepository.Add(entity);
        }

        public Task<int> AddAsync(T entity)
        {
            return _writeonlyRepository.AddAsync(entity);
        }

        public int Add(IEnumerable<T> entities)
        {
            return _writeonlyRepository.Add(entities);
        }

        public Task<int> AddAsync(IEnumerable<T> entities)
        {
            return _writeonlyRepository.AddAsync(entities);
        }

        public int Update(T entity)
        {
            return _writeonlyRepository.Update(entity);
        }

        public Task<int> UpdateAsync(T entity)
        {
            return _writeonlyRepository.UpdateAsync(entity);
        }

        public int Update(IEnumerable<T> entities)
        {
            return _writeonlyRepository.Update(entities);
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            return _writeonlyRepository.UpdateAsync(entities);
        }

        public int Delete(T entity, bool isHardDelete = false)
        {
            return _writeonlyRepository.Delete(entity, isHardDelete);
        }

        public Task<int> DeleteAsync(T entity, bool isHardDelete = false)
        {
            return _writeonlyRepository.DeleteAsync(entity, isHardDelete);
        }

        public int Delete(IEnumerable<T> entities, bool isHardDelete = false)
        {
            return _writeonlyRepository.Delete(entities, isHardDelete);
        }

        public Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false)
        {
            return _writeonlyRepository.DeleteAsync(entities, isHardDelete);
        }

        public int Save(IEnumerable<T> entities, bool isHardDelete = false)
        {
            return _writeonlyRepository.Save(entities, isHardDelete);
        }

        public Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false)
        {
            return _writeonlyRepository.SaveAsync(entities, isHardDelete);
        }

        #endregion
    }
}