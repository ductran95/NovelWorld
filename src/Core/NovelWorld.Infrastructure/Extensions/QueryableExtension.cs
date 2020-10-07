using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NovelWorld.Common;
using NovelWorld.Infrastructure.Exceptions;

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
    }
}