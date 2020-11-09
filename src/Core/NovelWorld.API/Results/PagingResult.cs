using System.Collections.Generic;
using NovelWorld.Data.Responses;

namespace NovelWorld.API.Results
{
    public class PagingResult<T>: Result<PagingResponse<T>>
    {
        public new static PagingResult<T> Create(PagingResponse<T> data)
        {
            return new PagingResult<T>
            {
                Success = true,
                Data = data
            };
        }
    }
}