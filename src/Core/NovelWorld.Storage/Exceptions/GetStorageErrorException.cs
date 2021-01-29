using System;

namespace NovelWorld.Storage.Exceptions
{
    public class GetStorageErrorException: Exception
    {
        public string Path { get; set; }

        public GetStorageErrorException(string path = null, string message = null, Exception innerException = null): base(message, innerException)
        {
            Path = path;
        }
    }
}