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
    public interface IAuthorQuery: IQuery
    {
        Task<AuthorDetailResponse> GetById(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AuthorGeneralResponse>> GetAll(CancellationToken cancellationToken = default);
        Task<PagingResponse<AuthorGeneralResponse>> GetByPaging(PagingRequest request, CancellationToken cancellationToken = default);
    }
}