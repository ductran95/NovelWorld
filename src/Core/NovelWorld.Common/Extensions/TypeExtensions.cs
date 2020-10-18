using System;
using System.Linq;
using NovelWorld.Data.Constants;

namespace NovelWorld.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsConvertible(this Type type)
        {
            return DefaultValues.ConvertibleTypes.Contains(type);
        }
    }
}