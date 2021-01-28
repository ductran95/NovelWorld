using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Book
{
    public sealed class UpdateBookRequestHandler : CommandHandler<UpdateBookRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public UpdateBookRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateBookRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, book);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}