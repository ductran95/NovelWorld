using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFramework.Contexts;
using NovelWorld.Infrastructure.Repositories;

namespace NovelWorld.Infrastructure.EntityFramework.Repositories
{
    public class WriteonlyRepository<T>: IWriteonlyRepository<T> where T: Entity
    {
        protected readonly EntityContext _context;
        protected readonly DbSet<T> _dbSet;
        public WriteonlyRepository(EntityContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        
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
    }
}