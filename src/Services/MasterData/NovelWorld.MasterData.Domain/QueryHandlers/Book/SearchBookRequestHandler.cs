using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.DTO;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.Extensions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Book
{
    public sealed class SearchBookRequestHandler : QueryHandler<SearchBookRequest, PagedData<BookGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public SearchBookRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<SearchBookRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<PagedData<BookGeneralResponse>> Handle(SearchBookRequest request, CancellationToken cancellationToken)
        {
            var books = _dbContext.Books.AsNoTracking();
            books = books.Filter(request.Filters).Sort(request.Sorts);
            var booksPaged = await books.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagedData<BookGeneralResponse>>(booksPaged);
        }
    }
}