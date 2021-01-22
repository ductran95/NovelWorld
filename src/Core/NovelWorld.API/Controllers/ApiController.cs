using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Results;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.DTO;
using NovelWorld.Mediator;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
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

        public Result<T> Result<T>(T data, int statusCode = Status200OK)
        {
            Response.StatusCode = statusCode;
            
            return new Result<T>
            {
                Success = true,
                Data = data
            };
        }
        
        public PagingResult<T> PagingResult<T>(PagedData<T> data, int statusCode = Status200OK)
        {
            Response.StatusCode = statusCode;
            
            return new PagingResult<T>
            {
                Success = true,
                Data = data
            };
        }
        
        public ListingResult<T> ListingResult<T>(IEnumerable<T> data, int statusCode = Status200OK)
        {
            Response.StatusCode = statusCode;
            
            return new ListingResult<T>
            {
                Success = true,
                Data = data
            };
        }
    }
}