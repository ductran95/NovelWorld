using System;
using System.Collections.Generic;
using System.Net;
using NovelWorld.Data.DTO;

namespace NovelWorld.Common.Exceptions
{
    public class HttpException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public HttpException(HttpStatusCode statusCode, IEnumerable<Error> errors, string message = "", Exception innerException = null): base(message, innerException)
        {
            StatusCode = statusCode;
            Errors = errors ?? new List<Error>(); // Make sure that list errors can not null
        }
    }
}