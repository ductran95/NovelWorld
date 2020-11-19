using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.Queries.Implements;
using NovelWorld.MasterData.Domain.Queries.Abstractions;

namespace NovelWorld.MasterData.Domain.Queries.Implements
{
    public class BookQuery: Query, IBookQuery
    {
        public BookQuery(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext
            ) : base(mapper, logger, authContext)
        {
        }
    }
}