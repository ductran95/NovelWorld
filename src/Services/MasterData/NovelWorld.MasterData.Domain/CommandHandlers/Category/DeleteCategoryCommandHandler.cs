using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class DeleteCategoryCommandHandler : CommandHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public DeleteCategoryCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<DeleteCategoryCommandHandler> logger, 
            IAuthContext authContext,
            ICategoryRepository categoryRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _categoryRepository = categoryRepository;
        }


        public override async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(request.Id);
            }

            _ = await _categoryRepository.DeleteAsync(category, cancellationToken: cancellationToken);
            return true;
        }
    }
}