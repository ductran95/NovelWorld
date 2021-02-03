using System;

namespace NovelWorld.MasterData.Data.Responses
{
    public class ChapterGeneralResponse
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
    }
}