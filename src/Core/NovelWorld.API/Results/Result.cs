using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using NovelWorld.Utility;
using NovelWorld.Data.DTO;

namespace NovelWorld.API.Results
{
    public class Result<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public IEnumerable<Error> Errors { get; set; }
    }
}