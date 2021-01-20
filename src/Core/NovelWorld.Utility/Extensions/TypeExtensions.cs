using System;
using System.Collections;
using System.Linq;
using NovelWorld.Data.Constants;

namespace NovelWorld.Utility.Extensions
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
        
        public static Type GetItemType(this Type enumerableType)
        {
            var genericTypes = enumerableType.GetGenericArguments();

            // List
            if (genericTypes.Any())
            {
                return genericTypes.LastOrDefault(); // get last for select query
            }
            else
            {
                // Array
                var elementType = enumerableType.GetElementType();
                if (elementType != null)
                {
                    return elementType;
                }
            }

            throw new NotImplementedException();
        }

        public static bool IsIEnumerable(this Type type)
        {
            var isImplementIEnumerable = !type.Equals(typeof(string)) && type.GetInterfaces().Any(x => x.IsGenericType && x.GetInterfaces().Contains(typeof(IEnumerable)));
            return isImplementIEnumerable;
        }
    }
}