using System;

namespace NovelWorld.Storage.Extensions
{
    public class StorageItemExistedException: Exception
    {
        public string Path { get; set; }

        public StorageItemExistedException(string path = null, string message = null, Exception innerException = null): base(message, innerException)
        {
            Path = path;
        }
    }
}