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
    public class EfCoreWriteonlyRepository<TContext, T>: IEfCoreWriteonlyRepository<T> where TContext: EfCoreEntityContext where T: Entity
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

        public async Task<int> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            Add(entity);
            return await SaveChangesAsync(cancellationToken);
        }

        public int Add(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
            return entities.Count();
        }

        public async Task<int> AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            Add(entities);
            return await SaveChangesAsync(cancellationToken);
        }

        public int Update(T entity)
        {
            _dbSet.Update(entity);
            return 1;
        }

        public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            Update(entity);
            return await SaveChangesAsync(cancellationToken);
        }

        public int Update(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
            return entities.Count();
        }

        public async Task<int> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            Update(entities);
            return await SaveChangesAsync(cancellationToken);
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

        public async Task<int> DeleteAsync(T entity, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            Delete(entity);
            return await SaveChangesAsync(cancellationToken);
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

        public async Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            Delete(entities);
            return await SaveChangesAsync(cancellationToken);
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

        public async Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default)
        {
            Save(entities);
            return await SaveChangesAsync(cancellationToken);
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