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
using NovelWorld.MasterData.Data.Responses.Chapter;
using NovelWorld.MasterData.Domain.Queries.Chapter;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Chapter
{
    public sealed class GetAllChapterRequestHandler : QueryHandler<GetAllChapterRequest, List<ChapterGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetAllChapterRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetAllChapterRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<ChapterGeneralResponse>> Handle(GetAllChapterRequest request, CancellationToken cancellationToken)
        {
            var chapters = await _dbContext.Chapters.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<List<ChapterGeneralResponse>>(chapters);
        }
    }
}