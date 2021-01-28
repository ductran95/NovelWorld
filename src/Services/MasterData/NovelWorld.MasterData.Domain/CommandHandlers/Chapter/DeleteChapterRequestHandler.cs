using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Chapter;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Chapter
{
    public sealed class DeleteChapterRequestHandler : CommandHandler<DeleteChapterRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public DeleteChapterRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteChapterRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(DeleteChapterRequest request, CancellationToken cancellationToken)
        {
            var chapter = await _dbContext.Chapters.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (chapter == null)
            {
                throw new NotFoundException(request.Id);
            }

            chapter.IsDeleted = true;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}