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
    public sealed class DeleteBookCommandHandler : CommandHandler<DeleteBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        
        public DeleteBookCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteBookCommandHandler> logger, 
            IAuthContext authContext,
            IBookRepository bookRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _bookRepository = bookRepository;
        }


        public override async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }

            _ = await _bookRepository.DeleteAsync(book, cancellationToken: cancellationToken);
            return true;
        }
    }
}