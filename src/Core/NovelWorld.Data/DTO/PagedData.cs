using System.Collections.Generic;
using NovelWorld.Data.Responses;

namespace NovelWorld.Data.DTO
{
    public class PagedData<T>: IResponse
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}