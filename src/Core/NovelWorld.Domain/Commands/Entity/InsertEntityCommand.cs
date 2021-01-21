using System;

namespace NovelWorld.Domain.Commands.Entity
{
    public class InsertEntityCommand<T>: Command<Guid>
    {
        public T Entity { get; set; }
    }
}