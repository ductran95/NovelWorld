using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Domain.Commands.Author;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Author
{
    public sealed class CreateAuthorRequestHandler : CommandHandler<CreateAuthorRequest, Guid>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public CreateAuthorRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateAuthorRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Guid> Handle(CreateAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Data.Entities.Author>(request);
            _ = await _dbContext.Authors.AddAsync(author, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return author.Id;
        }
    }
}