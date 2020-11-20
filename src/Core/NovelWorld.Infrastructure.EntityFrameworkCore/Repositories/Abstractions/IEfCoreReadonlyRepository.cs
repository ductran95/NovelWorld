using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using NovelWorld.Data.Entities;
using NovelWorld.Infrastructure.Repositories.Abstractions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Repositories.Abstractions
{
    public interface IEfCoreReadonlyRepository<T> : IReadonlyRepository<T> where T: Entity
    {
        T GetById(Guid id, Expression<Func<T, object>> includes = null, bool track = false);
        Task<T> GetByIdAsync(Guid id, Expression<Func<T, object>> includes = null, bool track = false, CancellationToken cancellationToken = default);
        T GetSingle(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null, bool track = false);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null, bool track = false, CancellationToken cancellationToken = default);
        IQueryable<T> GetAll(Expression<Func<T, object>> includes = null, bool track = false);
        IQueryable<T> GetMultiple(Expression<Func<T, bool>> condition, Expression<Func<T, object>> includes = null, bool track = false);
        IQueryable<T> GetAllIncludeDeleted(Expression<Func<T, object>> includes = null, bool track = false);
    }
}