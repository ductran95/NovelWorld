using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EfCoreGetListEntitiesByIdQueryHandler<T>: QueryHandler<GetListEntitiesByIdQuery<T>, List<T>> where T: Data.Entities.Entity
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public EfCoreGetListEntitiesByIdQueryHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EfCoreGetListEntitiesByIdQueryHandler<T>> logger, 
            IAuthContext authContext,
            DbContext dbContext
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public override async Task<List<T>> Handle(GetListEntitiesByIdQuery<T> request, CancellationToken cancellationToken)
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

            if (request.Ids != null)
            {
                return await query.Where(x => request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
            }
            
            if (request.Condition != null)
            {
                return await query.Where(request.Condition).ToListAsync(cancellationToken: cancellationToken);
            }

            return default;
        }
    }
}