using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions
{
    public interface IEfCoreRepository<T>: IRepository<T>, IEfCoreReadonlyRepository<T>, IEfCoreWriteonlyRepository<T> where T: Entity
    {
        
    }
}