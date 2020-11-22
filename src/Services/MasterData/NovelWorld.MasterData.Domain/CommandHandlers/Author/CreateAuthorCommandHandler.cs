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
    public sealed class CreateAuthorCommandHandler : CommandHandler<CreateAuthorCommand, Guid>
    {
        private readonly IAuthorRepository _authorRepository;
        
        public CreateAuthorCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateAuthorCommandHandler> logger, 
            IAuthContext authContext,
            IAuthorRepository authorRepository
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _authorRepository = authorRepository;
        }


        public override async Task<Guid> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = _mapper.Map<Author>(request);
            _ = await _authorRepository.AddAsync(author, cancellationToken);
            return author.Id;
        }
    }
}