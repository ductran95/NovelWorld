using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.Reader.Data.Entities
{
    public class ReadingHistory: Entity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        
        public int? ChapterNumber { get; set; }
        public int? PageNumber { get; set; }
    }
}