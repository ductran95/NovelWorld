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
using NovelWorld.MasterData.Domain.Queries.Author;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Author
{
    public sealed class GetPagingAuthorRequestHandler : QueryHandler<GetPagingAuthorRequest, PagedData<AuthorGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetPagingAuthorRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetPagingAuthorRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PagedData<AuthorGeneralResponse>> Handle(GetPagingAuthorRequest request, CancellationToken cancellationToken)
        {
            var authors = _dbContext.Authors.AsNoTracking();
            authors = authors.Filter(request.Filters).Sort(request.Sorts);
            var authorsPaged = await authors.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagedData<AuthorGeneralResponse>>(authorsPaged);
        }
    }
}