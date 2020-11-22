using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NovelWorld.Common;
using NovelWorld.Data.Requests;
using NovelWorld.Infrastructure.Exceptions;
using NovelWorld.Common.Extensions;

namespace NovelWorld.Infrastructure.Extensions
{
    public static class QueryableExtension
    {
        /// <summary>
        /// Get include properties from Expression
        /// only 1 level
        /// </summary>
        /// <param name="includes"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="IncludeTooDeepException"></exception>
        public static IEnumerable<string> GetIncludeProperties<T>(this Expression<Func<T, object>> includes)
            where T : class
        {
            Ensure.NotNull(includes);

            var result = new List<string>();

            if (includes is LambdaExpression lambdaExp)
            {
                if (lambdaExp.Body is NewExpression newExp)
                {
                    var data = newExp.Arguments;

                    foreach (var exp in data)
                    {
                        var memberExp = exp as MemberExpression;

                        if (memberExp != null)
                        {
                            if (!(memberExp.Expression is ParameterExpression))
                            {
                                throw new IncludeTooDeepException();
                            }
                            
                            result.Add(memberExp.Member.Name);
                        }
                    }
                }
                else if (lambdaExp.Body is MemberExpression memberExp)
                {
                    if (!(memberExp.Expression is ParameterExpression))
                    {
                        throw new IncludeTooDeepException();
                    }
                    result.Add(memberExp.Member.Name);
                }
            }
            
            return result;
        }
        
        public static IQueryable<T> Filter<T>(this IQueryable<T> query, IEnumerable<FilterRequest> filters)
            where T : class
        {
            if (filters == null || !filters.Any())
            {
                return query;
            }
            
            Ensure.NotContainNull(filters);
            
            var exp = filters.ToExpression<T>();
            
            return query.Where(exp);
        }
        
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, IEnumerable<SortRequest> sorts)
            where T : class
        {
            if (sorts == null || !sorts.Any())
            {
                return query;
            }
            
            Ensure.NotContainNull(sorts);

            var firstSort = sorts.FirstOrDefault();
            IOrderedQueryable<T> result = null;

            // ReSharper disable once PossibleNullReferenceException
            if (firstSort.Asc)
            {
                result = query.OrderBy(firstSort.Field);
            }
            else
            {
                result = query.OrderByDescending(firstSort.Field);
            }

            for (int i = 1; i < sorts.Count(); i++)
            {
                var sort = sorts.ElementAt(i);
                if (sort.Asc)
                {
                    result = result.ThenBy(sort.Field);
                }
                else
                {
                    result = result.ThenByDescending(sort.Field);
                }
            }
            
            return result;
        }
        
        public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int page, int pageSize)
            where T : class
        {
            Ensure.NotNullOrEmpty(page);
            Ensure.NotNullOrEmpty(pageSize);

            var skip = (page - 1) * pageSize;
            
            return query.Skip(skip).Take(pageSize);
        }
        
    }
}