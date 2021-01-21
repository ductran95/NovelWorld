using System;
using System.Linq.Expressions;

namespace NovelWorld.Domain.Queries.Entity
{
    public class GetEntityByIdQuery<T>: Query<T>
    {
        public Guid? Id { get; set; }
        public Expression<Func<T, bool>> Condition { get; set; }

        public bool IncludeDeleted { get; set; } = false;
        public bool NonTracking { get; set; } = true;
    }
}