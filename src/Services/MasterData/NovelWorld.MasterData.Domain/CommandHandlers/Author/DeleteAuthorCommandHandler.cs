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
    public sealed class DeleteAuthorCommandHandler : CommandHandler<DeleteAuthorCommand>
    {
        private readonly IAuthorRepository _authorRepository;
        
        public DeleteAuthorCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteAuthorCommandHandler> logger, 
            IAuthContext authContext,
            IAuthorRepository authorRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _authorRepository = authorRepository;
        }


        public override async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(request.Id);
            }

            _ = await _authorRepository.DeleteAsync(author, cancellationToken: cancellationToken);
            return true;
        }
    }
}