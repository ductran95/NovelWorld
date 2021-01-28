using System;
using System.Collections.Generic;
using NovelWorld.Data.Entities;
using NovelWorld.MasterData.Data.Enums;

namespace NovelWorld.MasterData.Data.Entities
{
    public class Book: Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public BookStatusEnum Status { get; set; }
        public float Rate { get; set; }
        public string Cover { get; set; }
        public Guid AuthorId { get; set; }
        
        public virtual Author Author { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Chapter> Chapters { get; set; }
    }
}