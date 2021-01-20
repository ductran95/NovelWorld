using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NovelWorld.Utility;
using NovelWorld.Data.Responses;
using NovelWorld.Infrastructure.Extensions;

namespace NovelWorld.Infrastructure.EntityFrameworkCore.Extensions
{
    public static class EfCoreQueryableExtension
    {
        public static IQueryable<T> Includes<T>(this IQueryable<T> query, Expression<Func<T, object>> includes)
            where T : class
        {
            Ensure.NotNull(includes);

            var includeProps = includes.GetIncludeProperties();

            foreach (var prop in includeProps)
            {
                query = query.Include(prop);
            }

            return query;
        }

        
        public static async Task<PagingResponse<T>> ToPaging<T>(this IQueryable<T> query, int page, int pageSize)
            where T : class
        {
            Ensure.NotNullOrEmpty(page);
            Ensure.NotNullOrEmpty(pageSize);

            var result = new PagingResponse<T>();
            result.Page = page;
            result.PageSize = pageSize;
            result.Total = await query.CountAsync();
            result.TotalPage = result.Total / result.PageSize + (result.Total % result.PageSize > 0 ? 1 : 0);
            result.Data = await query.GetPage(page, pageSize).ToListAsync();
            
            return result;
        }
    }
}