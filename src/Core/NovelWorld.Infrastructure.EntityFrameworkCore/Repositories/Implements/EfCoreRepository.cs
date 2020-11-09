using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements
{
    public class EfCoreRepository<TContext, T>: IRepository<T> where TContext: EfCoreEntityContext where T: Entity
    {
        protected readonly EfCoreEntityContext _context;
        protected readonly DbSet<T> _dbSet;
        public EfCoreRepository(EfCoreEntityContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        #region Read

        public T GetById(Guid id, Expression<Func<T, object>> includes = null)
        {
            return GetSingle(x => x.Id == id, includes);
        }
        
        public Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>> includes = null)
        {
            return GetSingleAsync(x => x.Id == id, includes);
        }

        public T GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
        {
            return GetAll(includes).Where(condition).FirstOrDefault();
        }
        
        public Task<T> GetSingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null)
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

        #endregion

        #region Write

        public int Add(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChanges();
        }

        public Task<int> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync();
        }

        public int Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return _context.SaveChanges();
        }

        public Task<int> AddAsync(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return _context.SaveChangesAsync();
        }

        public int Update(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChanges();
        }

        public Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return _context.SaveChangesAsync();
        }

        public int Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return _context.SaveChanges();
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return _context.SaveChangesAsync();
        }

        public int Delete(T entity, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.Remove(entity);
                return _context.SaveChanges();
            }
            else
            {
                entity.IsDeleted = true;
                return Update(entity);
            }
        }

        public Task<int> DeleteAsync(T entity, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.Remove(entity);
                return _context.SaveChangesAsync();
            }
            else
            {
                entity.IsDeleted = true;
                return UpdateAsync(entity);
            }
        }

        public int Delete(IEnumerable<T> entities, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.RemoveRange(entities);
                return _context.SaveChanges();
            }
            else
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }
                return Update(entities);
            }
        }

        public Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.RemoveRange(entities);
                return _context.SaveChangesAsync();
            }
            else
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }
                return UpdateAsync(entities);
            }
        }

        public int Save(IEnumerable<T> entities, bool isHardDelete = false)
        {
            foreach (var entity in entities)
            {
                // Add
                if (entity.Id == Guid.Empty)
                {
                    _dbSet.Add(entity);
                }
                // Update
                else if(!entity.IsDeleted)
                {
                    _dbSet.Update(entity);
                }
                // Delete
                else
                {
                    if (isHardDelete)
                    {
                        _dbSet.Remove(entity);
                    }
                    else
                    {
                        _dbSet.Update(entity);
                    }
                }
            }

            return _context.SaveChanges();
        }

        public Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false)
        {
            foreach (var entity in entities)
            {
                // Add
                if (entity.Id == Guid.Empty)
                {
                    _dbSet.Add(entity);
                }
                // Update
                else if(!entity.IsDeleted)
                {
                    _dbSet.Update(entity);
                }
                // Delete
                else
                {
                    if (isHardDelete)
                    {
                        _dbSet.Remove(entity);
                    }
                    else
                    {
                        _dbSet.Update(entity);
                    }
                }
            }

            return _context.SaveChangesAsync();
        }

        #endregion
    }
}