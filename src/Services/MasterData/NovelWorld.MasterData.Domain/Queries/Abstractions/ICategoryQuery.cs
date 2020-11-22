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
    public interface ICategoryQuery: IQuery
    {
        Task<CategoryDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryGeneralResponse>> GetAll(CancellationToken cancellationToken = default);
        Task<PagingResponse<CategoryGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default);
    }
}