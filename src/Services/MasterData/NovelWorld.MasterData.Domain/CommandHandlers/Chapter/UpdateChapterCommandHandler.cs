using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class UpdateChapterCommandHandler : CommandHandler<UpdateChapterCommand>
    {
        private readonly IChapterRepository _chapterRepository;
        
        public UpdateChapterCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateChapterCommandHandler> logger, 
            IAuthContext authContext,
            IChapterRepository chapterRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _chapterRepository = chapterRepository;
        }


        public override async Task<bool> Handle(UpdateChapterCommand request, CancellationToken cancellationToken)
        {
            var chapter = await _chapterRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (chapter == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, chapter);
            
            _ = await _chapterRepository.UpdateAsync(chapter, cancellationToken: cancellationToken);
            return true;
        }
    }
}