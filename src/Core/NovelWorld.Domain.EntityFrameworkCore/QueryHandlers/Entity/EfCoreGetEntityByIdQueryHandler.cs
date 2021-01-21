using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Queries.Entity;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Mediator;
using NovelWorld.Utility.Attributes;

namespace NovelWorld.Domain.EntityFrameworkCore.QueryHandlers.Entity
{
    [NotAutoRegister]
    public class EfCoreGetEntityByIdQueryHandler<T>: QueryHandler<GetEntityByIdQuery<T>, T> where T: Data.Entities.Entity
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public EfCoreGetEntityByIdQueryHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EfCoreGetEntityByIdQueryHandler<T>> logger, 
            IAuthContext authContext,
            DbContext dbContext
            ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public override async Task<T> Handle(GetEntityByIdQuery<T> request, CancellationToken cancellationToken)
        {
            var query = _dbSet.AsQueryable();

            if (request.NonTracking)
            {
                query = query.AsNoTracking();
            }
            
            if (request.IncludeDeleted)
            {
                query = query.IgnoreQueryFilters();
            }
            
            if (request.Includes != null)
            {
                query = query.Includes(request.Includes);
            }

            if (request.Id != null)
            {
                return await query.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);
            }
            
            if (request.Condition != null)
            {
                return await query.FirstOrDefaultAsync(request.Condition, cancellationToken: cancellationToken);
            }

            return default;
        }
    }
}