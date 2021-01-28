using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Book
{
    public sealed class CreateBookRequestHandler : CommandHandler<CreateBookRequest, Guid>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public CreateBookRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateBookRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Guid> Handle(CreateBookRequest request, CancellationToken cancellationToken)
        {
            var book = _mapper.Map<Data.Entities.Book>(request);
            _ = await _dbContext.Books.AddAsync(book, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return book.Id;
        }
    }
}