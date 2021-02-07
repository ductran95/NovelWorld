using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Controllers;
using NovelWorld.API.Results;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Author;
using NovelWorld.MasterData.Domain.Commands.Author;
using NovelWorld.MasterData.Domain.Queries.Author;
using NovelWorld.Mediator;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.MasterData.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AuthorController : ApiController
    {
        public AuthorController(
            IWebHostEnvironment environment,
            IMediator mediator, 
            IMapper mapper, 
            ILogger<AuthorController> logger, 
            IAuthContext authContext
            ) : base(environment, mediator, mapper, logger, authContext)
        {
        }
        
        [HttpGet("{id}")]
        public async Task<Result<AuthorDetailResponse>> GetDetail(Guid id)
        {
            var request = new GetDetailAuthorRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpGet]
        public async Task<ListingResult<AuthorGeneralResponse>> GetAll()
        {
            var request = new GetAllAuthorRequest();

            var response = await _mediator.Send(request);
            
            return ListingResult(response);
        }
        
        [HttpPost("Search")]
        public async Task<PagingResult<AuthorGeneralResponse>> Search([FromBody] SearchAuthorRequest request)
        {
            var response = await _mediator.Send(request);

            return PagingResult(response);
        }
        
        [HttpPost]
        public async Task<Result<Guid>> Create([FromBody] CreateAuthorRequest request)
        {
            var response = await _mediator.Send(request);

            return Result(response, Status201Created);
        }
        
        [HttpPut("{id}")]
        public async Task<Result<bool>> Update(Guid id, [FromBody] UpdateAuthorRequest request)
        {
            if (id != request.Id)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryDataNotMatchBody);
            }
            
            request.Id = id;
            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(Guid id)
        {
            var request = new DeleteAuthorRequest()
            {
                Id = id
            };
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
    }
}