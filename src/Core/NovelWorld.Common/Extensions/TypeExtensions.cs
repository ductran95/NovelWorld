using System;
using System.Linq;
using NovelWorld.Data.Constants;

namespace NovelWorld.Common.Extensions
{
    public static class TypeExtensions
    {
        public static string GetGenericTypeName(this Type type)
        {
            string typeName;

            if (type.IsGenericType)
            {
                var genericTypes = string.Join(",", type.GetGenericArguments().Select(t => t.Name).ToArray());
                typeName = $"{type.Name.Remove(type.Name.IndexOf('`'))}<{genericTypes}>";
            }
            else
            {
                typeName = type.Name;
            }

            return typeName;
        }
        
        public static bool IsConvertible(this Type type)
        {
            return DefaultValues.ConvertibleTypes.Contains(type);
        }
    }
}