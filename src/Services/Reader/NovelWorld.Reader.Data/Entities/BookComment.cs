using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.Reader.Data.Entities
{
    public class BookComment: Entity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
        
        public Guid? ParentId { get; set; }
    }
}