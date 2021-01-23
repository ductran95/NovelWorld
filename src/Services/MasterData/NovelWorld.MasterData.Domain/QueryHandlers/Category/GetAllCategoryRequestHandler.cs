using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Category;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Category
{
    public sealed class GetAllCategoryRequestHandler : QueryHandler<GetAllCategoryRequest, List<CategoryGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetAllCategoryRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetAllCategoryRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<CategoryGeneralResponse>> Handle(GetAllCategoryRequest request, CancellationToken cancellationToken)
        {
            var categories = await _dbContext.Categories.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<List<CategoryGeneralResponse>>(categories);
        }
    }
}