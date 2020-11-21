using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NovelWorld.Common.Extensions
{
    // ReSharper disable once InconsistentNaming
    public static class IEnumerableExtension
    {
        public static async Task<IEnumerable<T1>> SelectManyAsync<T, T1>(this IEnumerable<Task<T>> enumeration,
            Func<T, IEnumerable<T1>> func)
        {
            var task = Task.WhenAll(enumeration);
            return (await task).SelectMany(s => func(s));
        }

        public static bool Contains<T>(this IEnumerable<T> source, IEnumerable<T> dest)
        {
            if (source == null && dest == null)
            {
                return true;
            }

            if (source == null || dest == null)
            {
                return false;
            }

            return dest.All(x => source.Contains(x));
        }

        public static Type GetItemType(this IEnumerable enumeration)
        {
            var enumType = enumeration.GetType();
            return enumType.GetItemType();
        }
    }
}