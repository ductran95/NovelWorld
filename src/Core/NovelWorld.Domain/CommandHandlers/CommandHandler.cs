using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.Commands;
using NovelWorld.Mediator;

namespace NovelWorld.Domain.CommandHandlers
{
    public abstract class CommandHandler<T> : MediatR.IRequestHandler<T, bool> where T : Command
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<CommandHandler<T>> _logger;
        protected readonly IAuthContext _authContext;

        public CommandHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<CommandHandler<T>> logger, 
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

    public abstract class CommandHandler<T, TResponse> : MediatR.IRequestHandler<T, TResponse> where T : Command<TResponse>
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<CommandHandler<T, TResponse>> _logger;
        protected readonly IAuthContext _authContext;

        public CommandHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<CommandHandler<T, TResponse>> logger, 
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
