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
    public sealed class UpdateBookCommentRequestHandler : CommandHandler<UpdateBookCommentRequest>
    {
        private readonly ReaderDbContext _dbContext;
        
        public UpdateBookCommentRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateBookCommentRequestHandler> logger, 
            IAuthContext authContext,
            ReaderDbContext dbContext
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }


        public override async Task<bool> Handle(UpdateBookCommentRequest request, CancellationToken cancellationToken)
        {
            var bookComment = await _dbContext.BookComments.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (bookComment == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, bookComment);
            
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}