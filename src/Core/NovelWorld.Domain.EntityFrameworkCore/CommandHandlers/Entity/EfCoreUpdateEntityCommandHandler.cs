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
    public class EfCoreUpdateEntityCommandHandler<T>: CommandHandler<UpdateEntityCommand<T>, int> where T: Data.Entities.Entity
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public EfCoreUpdateEntityCommandHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EfCoreUpdateEntityCommandHandler<T>> logger, 
            IAuthContext authContext,
            DbContext dbContext
        ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public override async Task<int> Handle(UpdateEntityCommand<T> request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                _dbSet.Update(request.Entity);
            }
            
            if (request.Entities != null)
            {
                _dbSet.UpdateRange(request.Entities);
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}