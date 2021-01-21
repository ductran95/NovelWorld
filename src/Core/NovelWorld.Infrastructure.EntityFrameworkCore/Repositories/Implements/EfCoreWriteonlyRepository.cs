using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.EntityFrameworkCore.Contexts;
using NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Implements
{
    public class EfCoreWriteonlyRepository<TContext, T>: IEfCoreWriteonlyRepository<T> where TContext: EfCoreEntityDbContext where T: Entity
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dbSet;
        public EfCoreWriteonlyRepository(TContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        
        public int Add(T entity)
        {
            _dbSet.Add(entity);
            return 1;
        }

        public Task<int> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Add(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public int Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities.Count();
        }

        public Task<int> AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.AddRange(entities);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public int Update(T entity)
        {
            _dbSet.Update(entity);
            return 1;
        }

        public Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public int Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return entities.Count();
        }

        public Task<int> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.UpdateRange(entities);
            return _context.SaveChangesAsync(cancellationToken);
        }

        public int Delete(T entity, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.Remove(entity);
                return 1;
            }
            else
            {
                entity.IsDeleted = true;
                return Update(entity);
            }
        }

        public Task<int> DeleteAsync(T entity, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            if (isHardDelete)
            {
                _dbSet.Remove(entity);
                return _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                entity.IsDeleted = true;
                return UpdateAsync(entity, cancellationToken);
            }
        }

        public int Delete(IEnumerable<T> entities, bool isHardDelete = false)
        {
            if (isHardDelete)
            {
                _dbSet.RemoveRange(entities);
                return entities.Count();
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

        public Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            if (isHardDelete)
            {
                _dbSet.RemoveRange(entities);
                return _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                foreach (var entity in entities)
                {
                    entity.IsDeleted = true;
                }
                return UpdateAsync(entities, cancellationToken);
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

            return entities.Count();
        }

        public Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default)
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

            return _context.SaveChangesAsync(cancellationToken);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}