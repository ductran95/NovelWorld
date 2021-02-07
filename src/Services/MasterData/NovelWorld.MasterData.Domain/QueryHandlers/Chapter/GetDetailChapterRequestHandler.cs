using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Chapter;
using NovelWorld.MasterData.Domain.Queries.Chapter;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Chapter
{
    public sealed class GetDetailChapterRequestHandler : QueryHandler<GetDetailChapterRequest, ChapterDetailResponse>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetDetailChapterRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetDetailChapterRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<ChapterDetailResponse> Handle(GetDetailChapterRequest request, CancellationToken cancellationToken)
        {
            var chapter = await _dbContext.Chapters
                .Include(x=>x.Book)
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (chapter == null)
            {
                throw new NotFoundException(request.Id);
            }

            return _mapper.Map<ChapterDetailResponse>(chapter);
        }
    }
}