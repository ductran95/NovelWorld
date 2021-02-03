using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;
using NovelWorld.Storage.Providers.Abstractions;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Book
{
    public sealed class GetDetailBookRequestHandler : QueryHandler<GetDetailBookRequest, BookDetailResponse>
    {
        private readonly MasterDataDbContext _dbContext;
        private readonly IStorageProvider _storageProvider;
        
        public GetDetailBookRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetDetailBookRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext,
            IStorageProvider storageProvider
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }

        public override async Task<BookDetailResponse> Handle(GetDetailBookRequest request, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books
                .Include(x=>x.Author)
                .Include(x=>x.Categories)
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }
            
            return _mapper.Map<BookDetailResponse>(book);
        }
    }
}