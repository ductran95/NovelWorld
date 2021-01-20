using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NovelWorld.Authentication.Contexts.Abstractions;
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
    public class BookQuery: Query, IBookQuery
    {
        private readonly IBookRepository _bookRepository;
        
        public BookQuery(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext,
            IBookRepository bookRepository
            ) : base(mapper, logger, authContext)
        {
            _bookRepository = bookRepository;
        }

        public async Task<BookDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var book = await _bookRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (book == null)
            {
                throw new NotFoundException(id);
            }

            return _mapper.Map<BookDetailResponse>(book);
        }

        public async Task<IEnumerable<BookGeneralResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var books = await _bookRepository.GetAll().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<BookGeneralResponse>>(books);
        }

        public async Task<PagingResponse<BookGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default)
        {
            var books = _bookRepository.GetAll();
            books = books.Filter(request.Filters).Sort(request.Sorts);
            var booksPaged = await books.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagingResponse<BookGeneralResponse>>(booksPaged);
        }
    }
}