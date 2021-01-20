using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
using NovelWorld.Domain.CommandHandlers;
using NovelWorld.MasterData.Data.Entities;
using NovelWorld.MasterData.Domain.Commands;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;
using NovelWorld.Mediator;

namespace NovelWorld.MasterData.Domain.CommandHandlers
{
    public sealed class CreateCategoryCommandHandler : CommandHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public CreateCategoryCommandHandler(
            IMediator mediator, 
            IMapper mapper,
            ILogger<CreateCategoryCommandHandler> logger, 
            IAuthContext authContext,
            ICategoryRepository categoryRepository
            ) : base(mediator,
            mapper, logger, authContext)
        {
            _categoryRepository = categoryRepository;
        }


        public override async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);
            _ = await _categoryRepository.AddAsync(category, cancellationToken);
            return category.Id;
        }
    }
}