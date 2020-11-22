using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Data.Requests;
using NovelWorld.Data.Responses;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.Queries.Implements;
using NovelWorld.Infrastructure.EntityFrameworkCore.Extensions;
using NovelWorld.Infrastructure.Extensions;
using NovelWorld.MasterData.Data.Responses;
using NovelWorld.MasterData.Domain.Queries.Abstractions;
using NovelWorld.MasterData.Infrastructure.Repositories.Abstracts;

namespace NovelWorld.MasterData.Domain.Queries.Implements
{
    public class CategoryQuery: Query, ICategoryQuery
    {
        private readonly ICategoryRepository _categoryRepository;
        
        public CategoryQuery(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext,
            ICategoryRepository categoryRepository
            ) : base(mapper, logger, authContext)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (category == null)
            {
                throw new NotFoundException(id);
            }

            return _mapper.Map<CategoryDetailResponse>(category);
        }

        public async Task<IEnumerable<CategoryGeneralResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetAll().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CategoryGeneralResponse>>(categories);
        }

        public async Task<PagingResponse<CategoryGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default)
        {
            var categories = _categoryRepository.GetAll();
            categories = categories.Filter(request.Filters).Sort(request.Sorts);
            var categoriesPaged = await categories.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagingResponse<CategoryGeneralResponse>>(categoriesPaged);
        }
    }
}