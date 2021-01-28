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
using NovelWorld.MasterData.Domain.Queries.Chapter;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Chapter
{
    public sealed class SearchChapterRequestHandler : QueryHandler<SearchChapterRequest, PagedData<ChapterGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public SearchChapterRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<SearchChapterRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PagedData<ChapterGeneralResponse>> Handle(SearchChapterRequest request, CancellationToken cancellationToken)
        {
            var chapters = _dbContext.Chapters.AsNoTracking();
            chapters = chapters.Filter(request.Filters).Sort(request.Sorts);
            var chaptersPaged = await chapters.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagedData<ChapterGeneralResponse>>(chaptersPaged);
        }
    }
}