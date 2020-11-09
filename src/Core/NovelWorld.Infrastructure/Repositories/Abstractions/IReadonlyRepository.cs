using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;

namespace NovelWorld.Infrastructure.Repositories.Abstractions
{
    public interface IReadonlyRepository<T> where T: Entity
    {
        T GetById(Guid id, Expression<Func<T, object>> includes = null);
        Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>> includes = null);
        T GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null);
        IQueryable<T> GetAll(Expression<Func<T, object>> includes = null);
        IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null);
    }
}