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
using NovelWorld.MasterData.Domain.Commands.Chapter;
using NovelWorld.MasterData.Domain.Queries.Chapter;
using NovelWorld.Mediator;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.MasterData.API.Controllers
{
    [Authorize]
    public class ChapterController : ApiController
    {
        public ChapterController(
            IWebHostEnvironment environment,
            IMediator mediator, 
            IMapper mapper, 
            ILogger<ChapterController> logger, 
            IAuthContext authContext
            ) : base(environment, mediator, mapper, logger, authContext)
        {
        }
        
        [HttpGet("{id}")]
        public async Task<Result<ChapterDetailResponse>> GetDetail(Guid id)
        {
            var request = new GetDetailChapterRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpGet]
        public async Task<ListingResult<ChapterGeneralResponse>> GetAll()
        {
            var request = new GetAllChapterRequest();

            var response = await _mediator.Send(request);

            return ListingResult(response);
        }
        
        [HttpPost("Search")]
        public async Task<PagingResult<ChapterGeneralResponse>> Search([FromBody] SearchChapterRequest request)
        {
            var response = await _mediator.Send(request);

            return PagingResult(response);
        }
        
        [HttpPost]
        public async Task<Result<Guid>> Create([FromBody] CreateChapterRequest request)
        {
            var response = await _mediator.Send(request);

            return Result(response, Status201Created);
        }
        
        [HttpPut("{id}")]
        public async Task<Result<bool>> Update(Guid id, [FromBody] UpdateChapterRequest request)
        {
            if (id != request.Id)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryIdNotMatchBody);
            }
            
            request.Id = id;
            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpDelete("{id}")]
        public async Task<Result<bool>> Delete(Guid id)
        {
            var request = new DeleteChapterRequest()
            {
                Id = id
            };
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
    }
}