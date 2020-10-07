using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.Repositories
{
    public interface IWriteonlyRepository<T> where T: Entity
    {
        int Add(T entity);
        Task<int> AddAsync(T entity);
        int Add(IEnumerable<T> entities);
        Task<int> AddAsync(IEnumerable<T> entities);
        int Update(T entity);
        Task<int> UpdateAsync(T entity);
        int Update(IEnumerable<T> entities);
        Task<int> UpdateAsync(IEnumerable<T> entities);
        int Delete(T entity, bool isHardDelete = false);
        Task<int> DeleteAsync(T entity, bool isHardDelete = false);
        int Delete(IEnumerable<T> entities, bool isHardDelete = false);
        Task<int> DeleteAsync(IEnumerable<T> entities, bool isHardDelete = false);
        int Save(IEnumerable<T> entities, bool isHardDelete = false);
        Task<int> SaveAsync(IEnumerable<T> entities, bool isHardDelete = false);
    }
}