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
        int Update(T entity);
        Task<int> UpdateAsync(T entity);
        int Delete(T entity);
        Task<int> DeleteAsync(T entity);
        int Save(IEnumerable<T> entities);
        Task<int> SaveAsync(IEnumerable<T> entities);
    }
}