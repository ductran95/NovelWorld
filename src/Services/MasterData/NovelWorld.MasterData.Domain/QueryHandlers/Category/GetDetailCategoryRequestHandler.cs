using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Category;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Category
{
    public sealed class GetDetailCategoryRequestHandler : QueryHandler<GetDetailCategoryRequest, CategoryDetailResponse>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetDetailCategoryRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetDetailCategoryRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<CategoryDetailResponse> Handle(GetDetailCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _dbContext.Categories
                .Include(x=>x.Books)
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(request.Id);
            }

            return _mapper.Map<CategoryDetailResponse>(category);
        }
    }
}