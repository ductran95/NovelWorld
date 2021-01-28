using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Book
{
    public sealed class GetAllBookRequestHandler : QueryHandler<GetAllBookRequest, List<BookGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetAllBookRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetAllBookRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<BookGeneralResponse>> Handle(GetAllBookRequest request, CancellationToken cancellationToken)
        {
            var books = await _dbContext.Books.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<List<BookGeneralResponse>>(books);
        }
    }
}