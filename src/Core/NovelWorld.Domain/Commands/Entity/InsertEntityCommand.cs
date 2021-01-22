using System.Collections.Generic;

namespace NovelWorld.Domain.Commands.Entity
{
    public class InsertEntityCommand<T>: ICommand<int> where T: Data.Entities.Entity
    {
        public T Entity { get; set; }
        public List<T> Entities { get; set; }
    }
}