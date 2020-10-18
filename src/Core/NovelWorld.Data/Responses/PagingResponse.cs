using System.Collections;
using System.Collections.Generic;

namespace NovelWorld.Data.Responses
{
    public class PagingResponse<T>: Response
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPage { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}