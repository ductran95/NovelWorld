using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Mediator;

namespace NovelWorld.API.Controllers
{
    public abstract class ApiController: ControllerBase
    {
        protected readonly IMediator _mediator;
        protected readonly IMapper _mapper;
        protected readonly ILogger<ApiController> _logger;
        protected readonly IAuthContext _authContext;
        
        public ApiController(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<ApiController> logger, 
            IAuthContext authContext
        )
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
            _authContext = authContext;
        }
    }
}