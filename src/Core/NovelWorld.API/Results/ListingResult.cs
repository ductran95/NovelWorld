using System.Collections;
using System.Collections.Generic;

namespace NovelWorld.API.Results
{
    public class ListingResult<T>: Result<IEnumerable<T>>
    {
        public new static ListingResult<T> Create(IEnumerable<T> data)
        {
            return new ListingResult<T>
            {
                Success = true,
                Data = data
            };
        }
    }
}