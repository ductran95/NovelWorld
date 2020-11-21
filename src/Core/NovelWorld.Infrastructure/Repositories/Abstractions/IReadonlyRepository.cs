using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.Repositories.Abstractions
{
    public interface IReadonlyRepository<T> where T: Entity
    {
        
    }
}