using System.Collections.Generic;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.Repositories;

namespace NovelWorld.Infrastructure.EntityFramework.Repositories
{
    public class WriteonlyRepository<T>: IWriteonlyRepository<T> where T: Entity
    {
        public int Add(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> AddAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public int Update(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> UpdateAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public int Delete(T entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> DeleteAsync(T entity)
        {
            throw new System.NotImplementedException();
        }

        public int Save(IEnumerable<T> entities)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> SaveAsync(IEnumerable<T> entities)
        {
            throw new System.NotImplementedException();
        }
    }
}