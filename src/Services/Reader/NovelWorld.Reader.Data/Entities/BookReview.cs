using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.Reader.Data.Entities
{
    public class BookReview: Entity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string Content { get; set; }
    }
}