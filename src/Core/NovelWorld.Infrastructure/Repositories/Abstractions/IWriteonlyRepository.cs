using System.Collections.Generic;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.Repositories.Abstractions
{
    public interface IWriteonlyRepository<T> where T: Entity
    {
        
    }
}