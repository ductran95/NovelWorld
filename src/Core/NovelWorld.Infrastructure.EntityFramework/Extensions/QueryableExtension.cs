using System;
using System.Linq;
using System.Linq.Expressions;
using NovelWorld.Common;

namespace NovelWorld.Infrastructure.EntityFramework.Extensions
{
    internal static class QueryableExtension
    {
        internal static IQueryable<T> Includes<T>(this IQueryable<T> query, Expression<Func<T, object>> includes)
        {
            Ensure.NotNull(includes);
            
            
            
            return query;
        }
    }
}