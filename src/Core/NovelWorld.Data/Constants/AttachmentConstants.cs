using System.Collections.Generic;

namespace NovelWorld.Data.Constants
{
    public static class AttachmentUploadMode
    {
        public const string Local = "Local";
        public const string AzureStorage = "AzureStorage";
    }

    public static class AttachmentConstants
    {
        public const long MaxSize = 2097152;
        public static readonly List<string> ImageExts = new List<string> { ".jpg", ".png", ".jpeg" };
    }
}
