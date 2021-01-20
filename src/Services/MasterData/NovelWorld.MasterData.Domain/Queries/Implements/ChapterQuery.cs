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
    public class ChapterQuery: Query, IChapterQuery
    {
        private readonly IChapterRepository _chapterRepository;
        
        public ChapterQuery(
            IMapper mapper, 
            ILogger<Query> logger, 
            IAuthContext authContext,
            IChapterRepository chapterRepository
            ) : base(mapper, logger, authContext)
        {
            _chapterRepository = chapterRepository;
        }

        public async Task<ChapterDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var chapter = await _chapterRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

            if (chapter == null)
            {
                throw new NotFoundException(id);
            }

            return _mapper.Map<ChapterDetailResponse>(chapter);
        }

        public async Task<IEnumerable<ChapterGeneralResponse>> GetAll(CancellationToken cancellationToken = default)
        {
            var chapters = await _chapterRepository.GetAll().ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<ChapterGeneralResponse>>(chapters);
        }

        public async Task<PagingResponse<ChapterGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default)
        {
            var chapters = _chapterRepository.GetAll();
            chapters = chapters.Filter(request.Filters).Sort(request.Sorts);
            var chaptersPaged = await chapters.ToPagingAsync(request.Page, request.PageSize, cancellationToken);
            return _mapper.Map<PagingResponse<ChapterGeneralResponse>>(chaptersPaged);
        }
    }
}