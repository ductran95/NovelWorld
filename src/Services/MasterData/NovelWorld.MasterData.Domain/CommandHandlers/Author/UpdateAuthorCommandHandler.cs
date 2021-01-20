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
    public sealed class UpdateAuthorCommandHandler : CommandHandler<UpdateAuthorCommand>
    {
        private readonly IAuthorRepository _authorRepository;
        
        public UpdateAuthorCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateAuthorCommandHandler> logger, 
            IAuthContext authContext,
            IAuthorRepository authorRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _authorRepository = authorRepository;
        }


        public override async Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _authorRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, author);
            
            _ = await _authorRepository.UpdateAsync(author, cancellationToken: cancellationToken);
            return true;
        }
    }
}