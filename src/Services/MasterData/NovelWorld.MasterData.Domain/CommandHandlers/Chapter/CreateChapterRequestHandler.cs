using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Domain.Commands.Chapter;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Chapter
{
    public sealed class CreateChapterRequestHandler : CommandHandler<CreateChapterRequest, Guid>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public CreateChapterRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateChapterRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Guid> Handle(CreateChapterRequest request, CancellationToken cancellationToken)
        {
            var chapter = _mapper.Map<Data.Entities.Chapter>(request);
            _ = await _dbContext.Chapters.AddAsync(chapter, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return chapter.Id;
        }
    }
}