using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;
using NovelWorld.Storage.Providers.Abstractions;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Book
{
    public sealed class UploadBookCoverRequestHandler : CommandHandler<UploadBookCoverRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        private readonly IStorageProvider _storageProvider;
        
        public UploadBookCoverRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UploadBookCoverRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext,
            IStorageProvider storageProvider
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }


        public override async Task<bool> Handle(UploadBookCoverRequest request, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }

            var fileExt = Path.GetExtension(request.Cover.FileName);
            var fileName = $"Book_Cover_{request.Id}{fileExt}";

            using (var stream = new MemoryStream())
            {
                await request.Cover.CopyToAsync(stream, cancellationToken);
                if (string.IsNullOrEmpty(book.Cover))
                {
                    book.Cover = await _storageProvider.Create(stream, fileName, cancellationToken);
                }
                else
                {
                    book.Cover = await _storageProvider.Update(book.Cover, stream, fileName, cancellationToken);
                }
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}