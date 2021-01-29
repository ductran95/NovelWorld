using System;

namespace NovelWorld.Storage.Exceptions
{
    public class SaveStorageErrorException: Exception
    {
        public string Path { get; set; }

        public SaveStorageErrorException(string path = null, string message = null, Exception innerException = null): base(message, innerException)
        {
            Path = path;
        }
    }
}