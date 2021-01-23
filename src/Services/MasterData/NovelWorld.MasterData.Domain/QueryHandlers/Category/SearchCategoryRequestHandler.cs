using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.DTO;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.Extensions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Category;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Category
{
    public sealed class SearchCategoryRequestHandler : QueryHandler<SearchCategoryRequest, PagedData<CategoryGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public SearchCategoryRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<SearchCategoryRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PagedData<CategoryGeneralResponse>> Handle(SearchCategoryRequest request, CancellationToken cancellationToken)
        {
            var categories = _dbContext.Categories.AsNoTracking();
            categories = categories.Filter(request.Filters).Sort(request.Sorts);
            var categoriesPaged = await categories.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagedData<CategoryGeneralResponse>>(categoriesPaged);
        }
    }
}