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
    public class EfCoreInsertEntityCommandHandler<T>: CommandHandler<InsertEntityCommand<T>, int> where T: Data.Entities.Entity
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;
        
        public EfCoreInsertEntityCommandHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EfCoreInsertEntityCommandHandler<T>> logger, 
            IAuthContext authContext,
            DbContext dbContext
            ) : base(mediator, mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public override async Task<int> Handle(InsertEntityCommand<T> request, CancellationToken cancellationToken)
        {
            if (request.Entity != null)
            {
                await _dbSet.AddAsync(request.Entity, cancellationToken);
            }
            
            if (request.Entities != null)
            {
                await _dbSet.AddRangeAsync(request.Entities, cancellationToken);
            }

            return await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}