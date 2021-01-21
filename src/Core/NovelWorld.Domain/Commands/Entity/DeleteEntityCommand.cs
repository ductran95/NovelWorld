using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NovelWorld.Domain.Commands.Entity
{
    public class DeleteEntityCommand<T>: Command<Guid>
    {
        public Guid? Id { get; set; }
        public Expression<Func<T, bool>> Condition { get; set; }
        public T Entity { get; set; }
        
        public bool HardDelete { get; set; } = false;
    }
}