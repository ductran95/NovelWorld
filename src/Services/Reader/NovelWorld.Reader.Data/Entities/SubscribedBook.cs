using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.Reader.Data.Entities
{
    public class SubscribedBook: Entity
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        
        public bool NotifyNewChapter { get; set; }
        public bool NotifyNewComment { get; set; }
    }
}