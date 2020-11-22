using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class UpdateBookCommandHandler : CommandHandler<UpdateBookCommand>
    {
        private readonly IBookRepository _bookRepository;
        
        public UpdateBookCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateBookCommandHandler> logger, 
            IAuthContext authContext,
            IBookRepository bookRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _bookRepository = bookRepository;
        }


        public override async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, book);
            
            _ = await _bookRepository.UpdateAsync(book, cancellationToken: cancellationToken);
            return true;
        }
    }
}