using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NovelWorld.Common.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtension
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<Task<T>> enumeration, Func<T, IEnumerable<T1>> func)
        {
            return (await Task.WhenAll(enumeration)).SelectMany(s => func(s));
        }
    }
}