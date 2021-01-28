using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Domain.Commands.Category;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Category
{
    public sealed class CreateCategoryRequestHandler : CommandHandler<CreateCategoryRequest, Guid>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public CreateCategoryRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateCategoryRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Guid> Handle(CreateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Data.Entities.Category>(request);
            _ = await _dbContext.Categories.AddAsync(category, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return category.Id;
        }
    }
}