using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NovelWorld.API.Controllers;
using NovelWorld.API.Results;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Data.Constants;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Mediator;
using NovelWorld.Reader.Data.Responses.BookComment;
using NovelWorld.Reader.Domain.Commands.BookComment;
using NovelWorld.Reader.Domain.Queries.BookComment;
using static Microsoft.AspNetCore.Http.StatusCodes;

namespace NovelWorld.Reader.API.Controllers
{
    [Authorize]
    [Route("api")]
    public class BookCommentController : ApiController
    {
        public BookCommentController(
            IWebHostEnvironment environment,
            IMediator mediator, 
            IMapper mapper, 
            ILogger<BookCommentController> logger, 
            IAuthContext authContext
            ) : base(environment, mediator, mapper, logger, authContext)
        {
        }
        
        [HttpGet("Book/{bookId}/Comments")]
        public async Task<ListingResult<BookCommentTreeViewResponse>> GetAllTreeView(Guid bookId)
        {
            var request = new GetBookCommentsTreeViewRequest
            {
                BookId = bookId
            };

            var response = await _mediator.Send(request);

            return ListingResult(response);
        }
        
        [HttpPost("Book/{bookId}/Comment")]
        public async Task<Result<Guid>> Create(Guid bookId, [FromBody] CreateBookCommentRequest request)
        {
            if (bookId != request.BookId)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryDataNotMatchBody);
            }
            
            request.BookId = bookId;
            
            var response = await _mediator.Send(request);

            return Result(response, Status201Created);
        }
        
        [HttpPut("Book/{bookId}/Comment/{id}")]
        public async Task<Result<bool>> Update(Guid bookId, Guid id, [FromBody] UpdateBookCommentRequest request)
        {
            if (id != request.Id || bookId != request.BookId)
            {
                throw new BadRequestException(message: CommonErrorMessages.QueryDataNotMatchBody);
            }
            
            request.Id = id;
            request.BookId = bookId;
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
        
        [HttpDelete("Book/{bookId}/Comment/{id}")]
        public async Task<Result<bool>> Delete(Guid bookId, Guid id)
        {
            var request = new DeleteBookCommentRequest
            {
                Id = id
            };
            
            var response = await _mediator.Send(request);

            return Result(response);
        }
    }
}