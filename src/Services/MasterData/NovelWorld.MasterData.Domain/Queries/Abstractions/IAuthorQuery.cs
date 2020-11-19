using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NovelWorld.Data.Requests;
using NovelWorld.Data.Responses;
using NovelWorld.Domain.Queries.Abstractions;
using NovelWorld.MasterData.Data.Responses.Author;

namespace NovelWorld.MasterData.Domain.Queries.Abstractions
{
    public interface IAuthorQuery: IQuery
    {
        Task<AuthorDetailResponse> GetById(Guid id);
        Task<IEnumerable<AuthorGeneralResponse>> GetAll();
        Task<PagingResponse<AuthorGeneralResponse>> GetByPaging(PagingRequest request);
    }
}