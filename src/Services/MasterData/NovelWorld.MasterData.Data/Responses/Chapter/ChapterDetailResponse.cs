using System;

namespace NovelWorld.MasterData.Data.Responses
{
    public class ChapterDetailResponse
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        
        public BookGeneralResponse Book { get; set; }
    }
}