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
using NovelWorld.MasterData.Domain.Queries.Author;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Author
{
    public sealed class GetAllAuthorRequestHandler : QueryHandler<GetAllAuthorRequest, List<AuthorGeneralResponse>>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetAllAuthorRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetAllAuthorRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<AuthorGeneralResponse>> Handle(GetAllAuthorRequest request, CancellationToken cancellationToken)
        {
            var authors = await _dbContext.Authors.AsNoTracking().ToListAsync(cancellationToken);
            return _mapper.Map<List<AuthorGeneralResponse>>(authors);
        }
    }
}