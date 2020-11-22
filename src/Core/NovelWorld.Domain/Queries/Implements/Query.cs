using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.Queries.Abstractions;

namespace NovelWorld.Domain.Queries.Implements
{
    public abstract class Query: IQuery
    {
        protected readonly IMapper _mapper;
        protected readonly ILogger<Query> _logger;
        protected readonly IAuthContext _authContext;
        
        public Query(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext
            )
        {
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }
    }
}
