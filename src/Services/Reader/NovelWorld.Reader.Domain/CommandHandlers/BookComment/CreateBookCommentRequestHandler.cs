using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Mediator;
using NovelWorld.Reader.Domain.Commands.BookComment;
using NovelWorld.Reader.Infrastructure.Contexts;

namespace NovelWorld.Reader.Domain.CommandHandlers.BookComment
{
    public sealed class CreateBookCommentRequestHandler : CommandHandler<CreateBookCommentRequest, Guid>
    {
        private readonly ReaderDbContext _dbContext;
        
        public CreateBookCommentRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateBookCommentRequestHandler> logger, 
            IAuthContext authContext,
            ReaderDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<Guid> Handle(CreateBookCommentRequest request, CancellationToken cancellationToken)
        {
            var bookComment = _mapper.Map<Data.Entities.BookComment>(request);
            _ = await _dbContext.BookComments.AddAsync(bookComment, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return bookComment.Id;
        }
    }
}