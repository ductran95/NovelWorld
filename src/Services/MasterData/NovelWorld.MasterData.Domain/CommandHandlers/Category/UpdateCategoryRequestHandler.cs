using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Category;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Category
{
    public sealed class UpdateCategoryRequestHandler : CommandHandler<UpdateCategoryRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public UpdateCategoryRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateCategoryRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(UpdateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, category);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}