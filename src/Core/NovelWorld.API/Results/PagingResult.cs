using System.Collections.Generic;
using NovelWorld.Data.DTO;
using NovelWorld.Data.Responses;

namespace NovelWorld.API.Results
{
    public class PagingResult<T>: Result<PagedData<T>>
    {
    }
}