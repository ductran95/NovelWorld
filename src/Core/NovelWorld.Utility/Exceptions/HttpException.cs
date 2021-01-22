using System;
using System.Collections.Generic;
using NovelWorld.Data.DTO;

namespace NovelWorld.Utility.Exceptions
{
    public class HttpException: Exception
    {
        public int StatusCode { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public HttpException(int statusCode, IEnumerable<Error> errors, string message = "", Exception innerException = null): base(message, innerException)
        {
            StatusCode = statusCode;
            Errors = errors ?? new List<Error>(); // Make sure that list errors can not null
        }
    }
}