using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Controllers;
using NovelWorld.API.Results;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Commands.Author;
using NovelWorld.MasterData.Domain.Queries.Author;
using NovelWorld.Mediator;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.MasterData.API.Controllers
{
    public class AuthorController : ApiController
    {
        public AuthorController(
            IMediator mediator, 
            IMapper mapper, 
            ILogger<AuthorController> logger, 
            IAuthContext authContext
            ) : base(mediator, mapper, logger, authContext)
        {
        }
        
        [HttpGet]
        public async Task<Result<AuthorDetailResponse>> GetDetail([FromQuery] Guid id)
        {
            var request = new GetDetailAuthorRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(request);

            return Result<AuthorDetailResponse>(response);
        }
        
        [HttpGet]
        public async Task<ListingResult<AuthorGeneralResponse>> GetAll()
        {
            var request = new GetAllAuthorRequest();

            var response = await _mediator.Send(request);

            return ListingResult<AuthorGeneralResponse>(response);
        }
        
        [HttpPost]
        public async Task<PagingResult<AuthorGeneralResponse>> Filter([FromBody] GetPagingAuthorRequest request)
        {
            var response = await _mediator.Send(request);

            return PagingResult<AuthorGeneralResponse>(response);
        }
        
        [HttpPost]
        public async Task<Result<Guid>> Create([FromBody] CreateAuthorRequest request)
        {
            var response = await _mediator.Send(request);

            return Result<Guid>(response, Status201Created);
        }
        
        [HttpPut]
        public async Task<Result<bool>> Update([FromQuery] Guid id, [FromBody] UpdateAuthorRequest request)
        {
            if (id != request.Id)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryIdNotMatchBody);
            }
            
            request.Id = id;
            var response = await _mediator.Send(request);

            return Result<bool>(response);
        }
        
        [HttpDelete]
        public async Task<Result<bool>> Delete([FromQuery] Guid id)
        {
            var request = new DeleteAuthorRequest()
            {
                Id = id
            };
            
            var response = await _mediator.Send(request);

            return Result<bool>(response);
        }
    }
}