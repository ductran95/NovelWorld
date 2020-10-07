using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Common;
using NovelWorld.Infrastructure.Extensions;

namespace NovelWorld.Infrastructure.EntityFramework.Extensions
{
    public static class QueryableExtension
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> query, Expression<Func<T, object>> includes) where T : class
        {
            Ensure.NotNull(includes);

            var includeProps = includes.GetIncludeProperties();

            foreach (var prop in includeProps)
            {
                query = query.Include(prop);
            }
            
            return query;
        }
    }
}