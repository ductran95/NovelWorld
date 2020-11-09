using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.Repositories.Abstractions
{
    public interface IRepository<T>: IReadonlyRepository<T>, IWriteonlyRepository<T> where T: Entity
    {
        
    }
}