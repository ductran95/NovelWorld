using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NovelWorld.Domain.Commands.Entity
{
    public class DeleteEntityCommand<T>: Command<int> where T: Data.Entities.Entity
    {
        public Guid? Id { get; set; }
        public List<Guid> Ids { get; set; }
        public T Entity { get; set; }
        public List<T> Entities { get; set; }
        public Expression<Func<T, bool>> Condition { get; set; }
        
        public bool HardDelete { get; set; } = false;
    }
}