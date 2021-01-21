using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NovelWorld.Domain.Queries.Entity
{
    public class GetQueryableEntitiesByIdQuery<T>: Query<IQueryable<T>> where T: Data.Entities.Entity
    {
        public IEnumerable<Guid> Ids { get; set; }
        public Expression<Func<T, bool>> Condition { get; set; }
        public Expression<Func<T, object>> Includes { get; set; }

        public bool IncludeDeleted { get; set; } = false;
        public bool NonTracking { get; set; } = true;
    }
}