using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.Events;
using NovelWorld.Mediator;

namespace NovelWorld.Domain.EventHandlers
{
    public abstract class EventHandler<T> : MediatR.INotificationHandler<T> where T : IEvent
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<EventHandler<T>> _logger;
        protected readonly IAuthContext _authContext;

        public EventHandler(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<EventHandler<T>> logger, 
            IAuthContext authContext
            )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }
        
        public abstract Task Handle(T notification, CancellationToken cancellationToken);
    }
}
