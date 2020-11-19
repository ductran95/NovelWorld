using System;
using NovelWorld.Data.Entities;

namespace NovelWorld.MasterData.Data.DTO
{
    public class Chapter: Entity
    {
        public int? Number { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        
        public virtual Book Book { get; set; }
    }
}