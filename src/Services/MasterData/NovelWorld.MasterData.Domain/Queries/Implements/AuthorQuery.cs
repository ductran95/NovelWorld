using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Implements;
using NovelWorld.Data.Requests;
using NovelWorld.Data.Responses;
using NovelWorld.Domain.Exceptions;
using NovelWorld.Domain.Queries.Implements;
using NovelWorld.MasterData.Data.Responses.Author;
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

        public async Task<AuthorDetailResponse> GetById(Guid id)
        {
            var author = await _authorRepository.GetByIdAsync(id);

            if (author == null)
            {
                throw new NotFoundException(id);
            }

            return _mapper.Map<AuthorDetailResponse>(author);
        }

        public async Task<IEnumerable<AuthorGeneralResponse>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PagingResponse<AuthorGeneralResponse>> GetByPaging(PagingRequest request)
        {
            throw new NotImplementedException();
        }
    }
}