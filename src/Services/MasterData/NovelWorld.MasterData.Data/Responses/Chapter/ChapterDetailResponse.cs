using System;
using NovelWorld.Data.Responses;
using NovelWorld.MasterData.Data.Responses.Book;

namespace NovelWorld.MasterData.Data.Responses.Chapter
{
    public class ChapterDetailResponse: EntityResponse
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        
        public BookGeneralResponse Book { get; set; }
    }
}