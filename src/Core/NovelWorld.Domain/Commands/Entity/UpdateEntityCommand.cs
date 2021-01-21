using System;
using System.Collections.Generic;

namespace NovelWorld.Domain.Commands.Entity
{
    public class UpdateEntityCommand<T>: Command<int> where T: Data.Entities.Entity
    {
        public T Entity { get; set; }
        public List<T> Entities { get; set; }
        
        public IEnumerable<string> FieldsToUpdate { get; set; }
    }
}