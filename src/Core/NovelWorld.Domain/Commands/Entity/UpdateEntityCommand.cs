using System;
using System.Collections.Generic;

namespace NovelWorld.Domain.Commands.Entity
{
    public class UpdateEntityCommand<T>: Command<Guid>
    {
        public T Entity { get; set; }
        public IEnumerable<string> FieldsToUpdate { get; set; }
    }
}