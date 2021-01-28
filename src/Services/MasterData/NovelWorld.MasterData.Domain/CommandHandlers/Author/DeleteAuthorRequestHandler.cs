using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Author;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Author
{
    public sealed class DeleteAuthorRequestHandler : CommandHandler<DeleteAuthorRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public DeleteAuthorRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteAuthorRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Authors.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(request.Id);
            }

            author.IsDeleted = true;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}