using System;

namespace NovelWorld.Storage.Exceptions
{
    public class StorageItemNotFoundException: Exception
    {
        public string Path { get; set; }

        public StorageItemNotFoundException(string path = null, string message = null, Exception innerException = null): base(message, innerException)
        {
            Path = path;
        }
    }
}