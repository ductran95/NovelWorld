using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands.Book;
using NovelWorld.MasterData.Domain.Configurations;
using NovelWorld.MasterData.Infrastructure.Contexts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers.Book
{
    public sealed class UploadBookCoverRequestHandler : CommandHandler<UploadBookCoverRequest>
    {
        private readonly MasterDataDbContext _dbContext;
        private readonly MasterDataAppSettings _appSetting;
        
        public UploadBookCoverRequestHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UploadBookCoverRequestHandler> logger, 
            IAuthContext authContext,
            MasterDataDbContext dbContext,
            IOptions<MasterDataAppSettings> appSetting
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _dbContext = dbContext;
            _appSetting = appSetting.Value;
        }


        public override async Task<bool> Handle(UploadBookCoverRequest request, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(x=>x.Id == request.Id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(request.Id);
            }

            var filePath = Path.Combine(_appSetting.StorageConfiguration.RootPath, "Cover");
            Directory.CreateDirectory(filePath);

            var fileExt = Path.GetExtension(request.Cover.FileName);
            filePath = Path.Combine(filePath, $"Book_Cover_{request.Id}{fileExt}");
            
            using (var stream = System.IO.File.Create(filePath))
            {
                await request.Cover.CopyToAsync(stream, cancellationToken);
            }

            book.Cover = filePath;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}