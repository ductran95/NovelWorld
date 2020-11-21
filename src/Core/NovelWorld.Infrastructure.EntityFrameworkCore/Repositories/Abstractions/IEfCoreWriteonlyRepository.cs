using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions
{
    public interface IEfCoreWriteonlyRepository<T>: IWriteonlyRepository<T> where T: Entity
    {
        int Add(T entity);
        Task<int> AddAsync(T entity, CancellationToken cancellationToken = default);
        int Add(IEnumerable<T> entities);
        Task<int> AddAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        int Update(T entity);
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        int Update(IEnumerable<T> entities);
        Task<int> UpdateAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        int Delete(T entity, bool isHardDelete = false);
        Task<int> DeleteAsync(T entity, bool isHardDelete = false, CancellationToken cancellationToken = default);
        int Delete(IEnumerable<T> entities, bool isHardDelete = false);
        Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default);
        int Save(IEnumerable<T> entities, bool isHardDelete = false);
        Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false, CancellationToken cancellationToken = default);
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}