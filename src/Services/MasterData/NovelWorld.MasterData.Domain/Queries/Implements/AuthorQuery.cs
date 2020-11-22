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
    public class AuthorQuery: Query, IAuthorQuery
    {
        private readonly IAuthorRepository _authorRepository;
        
        public AuthorQuery(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext,
            IAuthorRepository authorRepository
            ) : base(mapper, logger, authContext)
        {
            _authorRepository = authorRepository;
        }

        public async Task<AuthorDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var author = await _authorRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (author == null)
            {
                throw new NotFoundException(id);
            }

            return _mapper.Map<AuthorDetailResponse>(author);
        }

        public async Task<IEnumerable<AuthorGeneralResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var authors = await _authorRepository.GetAll().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<AuthorGeneralResponse>>(authors);
        }

        public async Task<PagingResponse<AuthorGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default)
        {
            var authors = _authorRepository.GetAll();
            authors = authors.Filter(request.Filters).Sort(request.Sorts);
            var authorsPaged = await authors.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagingResponse<AuthorGeneralResponse>>(authorsPaged);
        }
    }
}