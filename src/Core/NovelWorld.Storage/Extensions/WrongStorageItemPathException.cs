using System;

namespace NovelWorld.Storage.Extensions
{
    public class WrongStorageItemPathException: Exception
    {
        public string Path { get; set; }

        public WrongStorageItemPathException(string path = null, string message = null, Exception innerException = null): base(message, innerException)
        {
            Path = path;
        }
    }
}