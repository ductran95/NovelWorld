using System.Collections.Generic;
using NovelWorld.Data.Entities;

namespace NovelWorld.MasterData.Data.DTO
{
    public class Author: Entity
    {
        public string Name { get; set; }
        public string Summary { get; set; }
        
        public virtual ICollection<Book> Books { get; set; }
    }
}