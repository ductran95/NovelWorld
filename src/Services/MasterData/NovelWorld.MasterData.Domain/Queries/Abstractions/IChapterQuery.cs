using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NovelWorld.Data.Requests;
using NovelWorld.Data.Responses;
using NovelWorld.Domain.Queries.Abstractions;
using NovelWorld.MasterData.Data.Responses;

namespace NovelWorld.MasterData.Domain.Queries.Abstractions
{
    public interface IChapterQuery: IQuery
    {
        Task<ChapterDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ChapterGeneralResponse>> GetAll(CancellationToken cancellationToken = default);
        Task<PagingResponse<ChapterGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default);
    }
}