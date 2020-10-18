using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NovelWorld.Common.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IOrderedQueryableExtensions
    {
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string property)
        {
            return query.ApplyOrder(property, "OrderBy");
        }
        
        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string property)
        {
            return query.ApplyOrder(property, "OrderByDescending");
        }
        
        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string property)
        {
            return query.ApplyOrder(property, "ThenBy");
        }
        
        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string property)
        {
            return query.ApplyOrder(property, "ThenByDescending");
        }
        
        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> query, string property, string methodName)
        {
            string[] properties = property.Split('.');
            Type type = typeof(T);

            ParameterExpression arg = Expression.Parameter(type, "p");
            Expression exp = arg;

            foreach (var prop in properties)
            {
                PropertyInfo pi = type.GetProperty(prop);
                exp = Expression.Property(exp, pi);
                type = pi.PropertyType;
            }

            Type delegateType = typeof(Func<,>).MakeGenericType(typeof(T), type);
            LambdaExpression lambda = Expression.Lambda(delegateType, exp, arg);

            object result = typeof(Queryable).GetMethods().Single(method => method.Name == methodName &&
                                                                            method.IsGenericMethodDefinition &&
                                                                            method.GetGenericArguments().Length == 2 &&
                                                                            method.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), type)
                .Invoke(null, new object[] {query, lambda});

            return (IOrderedQueryable<T>)result;
        }
    }
}