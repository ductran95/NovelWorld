using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Mediator;
using NovelWorld.Reader.Domain.Commands.BookComment;
using NovelWorld.Reader.Infrastructure.Contexts;

namespace NovelWorld.Reader.Domain.CommandHandlers.BookComment
{
    public sealed class DeleteBookCommentRequestHandler : CommandHandler<DeleteBookCommentRequest>
    {
        private readonly ReaderDbContext _dbContext;
        
        public DeleteBookCommentRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteBookCommentRequestHandler> logger, 
            IAuthContext authContext,
            ReaderDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(DeleteBookCommentRequest request, CancellationToken cancellationToken)
        {
            var bookComment = await _dbContext.BookComments.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (bookComment == null)
            {
                throw new NotFoundException(request.Id);
            }

            bookComment.IsDeleted = true;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}