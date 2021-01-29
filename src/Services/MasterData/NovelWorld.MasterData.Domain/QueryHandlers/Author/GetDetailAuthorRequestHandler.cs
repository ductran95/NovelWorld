using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Author;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.QueryHandlers.Author
{
    public sealed class GetDetailAuthorRequestHandler : QueryHandler<GetDetailAuthorRequest, AuthorDetailResponse>
    {
        private readonly MasterDataDbContext _dbContext;
        
        public GetDetailAuthorRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetDetailAuthorRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<AuthorDetailResponse> Handle(GetDetailAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _dbContext.Authors
                .Include(x=>x.Books)
                .FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(request.Id);
            }

            return _mapper.Map<AuthorDetailResponse>(author);
        }
    }
}