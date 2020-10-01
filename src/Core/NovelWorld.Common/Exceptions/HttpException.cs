using System;
using System.Collections.Generic;
using System.Net;

namespace NovelWorld.Common.Exceptions
{
    public class HttpException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public IEnumerable<Error> Errors { get; set; }

        public HttpException(string message, HttpStatusCode statusCode, IEnumerable<Error> errors, Exception innerException = null): base(message, innerException)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}