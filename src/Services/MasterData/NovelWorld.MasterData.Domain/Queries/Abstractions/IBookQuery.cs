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
    public interface IBookQuery: IQuery
    {
        Task<BookDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BookGeneralResponse>> GetAll(CancellationToken cancellationToken = default);
        Task<PagingResponse<BookGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default);
    }
}