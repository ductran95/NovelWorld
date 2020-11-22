using System.Collections.Generic;
using NovelWorld.Data.Entities;

namespace NovelWorld.MasterData.Data.Entities
{
    public class Category: Entity
    {
        public string Name { get; set; }
        
        public virtual ICollection<Book> Books { get; set; }
    }
}