using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.Queries;
using NovelWorld.Mediator;

namespace NovelWorld.Domain.QueryHandlers
{
    public abstract class QueryHandler<T> : MediatR.IRequestHandler<T, bool> where T : IQuery
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<QueryHandler<T>> _logger;
        protected readonly IAuthContext _authContext;

        public QueryHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<QueryHandler<T>> logger, 
            IAuthContext authContext
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }
        
        public abstract Task<bool> Handle(T request, CancellationToken cancellationToken);
    }

    public abstract class QueryHandler<T, TResponse> : MediatR.IRequestHandler<T, TResponse> where T : IQuery<TResponse>
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<QueryHandler<T, TResponse>> _logger;
        protected readonly IAuthContext _authContext;

        public QueryHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<QueryHandler<T, TResponse>> logger, 
            IAuthContext authContext 
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }
        
        public abstract Task<TResponse> Handle(T request, CancellationToken cancellationToken);
    }
}
