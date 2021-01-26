using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Controllers;
using NovelWorld.API.Results;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Domain.Queries.Book;
using NovelWorld.Mediator;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.MasterData.API.Controllers
{
    public class BookController : ApiController
    {
        public BookController(
            IWebHostEnvironment environment,
            IMediator mediator, 
            IMapper mapper, 
            ILogger<BookController> logger, 
            IAuthContext authContext
            ) : base(environment, mediator, mapper, logger, authContext)
        {
        }
        
        [HttpGet]
        public async Task<Result<BookDetailResponse>> GetDetail([FromQuery] Guid id)
        {
            var request = new GetDetailBookRequest()
            {
                Id = id
            };

            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpGet]
        public async Task<ListingResult<BookGeneralResponse>> GetAll()
        {
            var request = new GetAllBookRequest();

            var response = await _mediator.Send(request);

            return ListingResult(response);
        }
        
        [HttpPost]
        public async Task<PagingResult<BookGeneralResponse>> Search([FromBody] SearchBookRequest request)
        {
            var response = await _mediator.Send(request);

            return PagingResult(response);
        }
        
        [HttpPost]
        public async Task<Result<Guid>> Create([FromBody] CreateBookRequest request)
        {
            var response = await _mediator.Send(request);

            return Result(response, Status201Created);
        }
        
        [HttpPut]
        public async Task<Result<bool>> Update([FromQuery] Guid id, [FromBody] UpdateBookRequest request)
        {
            if (id != request.Id)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryIdNotMatchBody);
            }
            
            request.Id = id;
            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpPut]
        public async Task<Result<bool>> UploadCover([FromQuery] Guid id, [FromForm] IFormFile cover)
        {
            var request = new UploadBookCoverRequest()
            {
                Id = id,
                Cover = cover
            };
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpDelete]
        public async Task<Result<bool>> Delete([FromQuery] Guid id)
        {
            var request = new DeleteBookRequest()
            {
                Id = id
            };
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
    }
}