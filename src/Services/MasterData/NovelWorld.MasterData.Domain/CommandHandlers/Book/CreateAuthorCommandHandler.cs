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
    public sealed class CreateBookCommandHandler : CommandHandler<CreateBookCommand, Guid>
    {
        private readonly IBookRepository _bookRepository;
        
        public CreateBookCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateBookCommandHandler> logger, 
            IAuthContext authContext,
            IBookRepository bookRepository
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _bookRepository = bookRepository;
        }


        public override async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Book>(request);
            _ = await _bookRepository.AddAsync(book, cancellationToken);
            return book.Id;
        }
    }
}