using System;
using System.Linq.Expressions;

namespace NovelWorld.Domain.Queries.Entity
{
    public class GetEntityByIdQuery<T>: IQuery<T> where T: Data.Entities.Entity
    {
        public Guid? Id { get; set; }
        public Expression<Func<T, bool>> Condition { get; set; }
        public Expression<Func<T, object>> Includes { get; set; }

        public bool IncludeDeleted { get; set; } = false;
        public bool NonTracking { get; set; } = true;
    }
}