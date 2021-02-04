using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.QueryHandlers;
using NovelWorld.Mediator;
using NovelWorld.Reader.Data.Responses.BookComment;
using NovelWorld.Reader.Domain.Queries.BookComment;
using NovelWorld.Reader.Infrastructure.Contexts;

namespace NovelWorld.Reader.Domain.QueryHandlers.BookComment
{
    public sealed class GetBookCommentsTreeViewRequestHandler : QueryHandler<GetBookCommentsTreeViewRequest, List<BookCommentTreeViewResponse>>
    {
        private readonly ReaderDbContext _dbContext;
        
        public GetBookCommentsTreeViewRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<GetBookCommentsTreeViewRequestHandler> logger, 
            IAuthContext authContext,
            ReaderDbContext dbContext
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<List<BookCommentTreeViewResponse>> Handle(GetBookCommentsTreeViewRequest request, CancellationToken cancellationToken)
        {
            var bookComments = await _dbContext.BookComments.AsNoTracking()
                .Where(x=>x.BookId == request.BookId)
                .OrderByDescending(x=> x.ModifiedOn)
                .ToListAsync(cancellationToken);

            var commentTree = _mapper.Map<List<BookCommentTreeViewResponse>>(bookComments);

            foreach (var comment in commentTree)
            {
                if (comment.ParentId != null)
                {
                    var parent = commentTree.FirstOrDefault(x => x.Id == comment.ParentId);
                    if (parent != null)
                    {
                        if (parent.Replies == null)
                        {
                            parent.Replies = new List<BookCommentTreeViewResponse>();
                        }
                        
                        parent.Replies.Add(comment);
                    }
                }
            }

            return commentTree.Where(x=>x.ParentId == null).ToList();
        }
    }
}