using System;
using NovelWorld.Data.Responses;

namespace NovelWorld.MasterData.Data.Responses.Chapter
{
    public class ChapterGeneralResponse: EntityResponse
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
    }
}