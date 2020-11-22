using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class CreateChapterCommandHandler : CommandHandler<CreateChapterCommand, Guid>
    {
        private readonly IChapterRepository _chapterRepository;
        
        public CreateChapterCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateChapterCommandHandler> logger, 
            IAuthContext authContext,
            IChapterRepository chapterRepository
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _chapterRepository = chapterRepository;
        }


        public override async Task<Guid> Handle(CreateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = _mapper.Map<Chapter>(request);
            _ = await _chapterRepository.AddAsync(chapter, cancellationToken);
            return chapter.Id;
        }
    }
}