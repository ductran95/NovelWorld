using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Commands.Entity;
using NovelWorld.Mediator;
using NovelWorld.Utility.Attributes;

namespace NovelWorld.Domain.EntityFrameworkCore.CommandHandlers.Entity
{
    [NotAutoRegister]
    public class EfCoreDeleteEntityCommandHandler<T>: CommandHandler<DeleteEntityCommand<T>, int> where T: Data.Entities.Entity
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public EfCoreDeleteEntityCommandHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EfCoreDeleteEntityCommandHandler<T>> logger, 
            IAuthContext authContext,
            DbContext dbContext
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public override async Task<int> Handle(DeleteEntityCommand<T> request, CancellationToken cancellationToken)
        {
            if (request.HardDelete)
            {
                if (request.Entity != null)
                {
                    _dbSet.Remove(request.Entity);
                }
            
                if (request.Entities != null)
                {
                    _dbSet.RemoveRange(request.Entities);
                }
                
                if (request.Id != null)
                {
                    var entity = await _dbSet.Where(x=>x.Id == request.Id.Value).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    _dbSet.Remove(entity);
                }
                
                if (request.Ids != null)
                {
                    var entities = await _dbSet.Where(x=>request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
                    _dbSet.RemoveRange(entities);
                }
                
                if (request.Condition != null)
                {
                    var entities = await _dbSet.Where(request.Condition).ToListAsync(cancellationToken: cancellationToken);
                    _dbSet.RemoveRange(entities);
                }
            }
            else
            {
                if (request.Entity != null)
                {
                    request.Entity.IsDeleted = false;
                    _dbSet.Update(request.Entity);
                }
            
                if (request.Entities != null)
                {
                    request.Entities.ForEach(x=>x.IsDeleted = false);
                    _dbSet.UpdateRange(request.Entities);
                }
                
                if (request.Id != null)
                {
                    var entity = await _dbSet.Where(x=>x.Id == request.Id.Value).FirstOrDefaultAsync(cancellationToken: cancellationToken);
                    entity.IsDeleted = false;
                    _dbSet.Update(entity);
                }
                
                if (request.Ids != null)
                {
                    var entities = await _dbSet.Where(x=>request.Ids.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
                    entities.ForEach(x=>x.IsDeleted = false);
                    _dbSet.UpdateRange(entities);
                }
                
                if (request.Condition != null)
                {
                    var entities = await _dbSet.Where(request.Condition).ToListAsync(cancellationToken: cancellationToken);
                    entities.ForEach(x=>x.IsDeleted = false);
                    _dbSet.UpdateRange(entities);
                }
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}