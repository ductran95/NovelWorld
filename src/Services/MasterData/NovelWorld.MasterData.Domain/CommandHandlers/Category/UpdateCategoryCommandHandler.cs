using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.Domain.Exceptions;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class UpdateCategoryCommandHandler : CommandHandler<UpdateCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public UpdateCategoryCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<UpdateCategoryCommandHandler> logger, 
            IAuthContext authContext,
            ICategoryRepository categoryRepository
        ) : base(mediator,
            mapper, logger, authContext)
        {
            _categoryRepository = categoryRepository;
        }


        public override async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(request.Id);
            }

            _mapper.Map(request, category);
            
            _ = await _categoryRepository.UpdateAsync(category, cancellationToken: cancellationToken);
            return true;
        }
    }
}