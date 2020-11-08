using System.Collections;
using System.Collections.Generic;
using NovelWorld.Common;
using NovelWorld.Data.DTO;

namespace NovelWorld.API.Results
{
    public class Result<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
        
        public static Result<T> Create(T data)
        {
            return new Result<T>
            {
                Success = true,
                Data = data
            };
        }
    }
}