using System;
using System.Linq;
using System.Reflection;

namespace NovelWorld.Common.Extensions
{
    public static class MethodInfoExtension
    {
        public static T GetCustomAttribute<T>(this MethodInfo method, bool inherit = true) where T: Attribute
        {
            var attributes = method.GetCustomAttributes(typeof(T), inherit);

            if(attributes.Any())
            {
                return (T)attributes[0];
            }
            else
            {
                return null;
            }
        }

        public static T GetCustomAttribute<T>(this Type type, bool inherit = true) where T : Attribute
        {
            var attributes = type.GetCustomAttributes(typeof(T), inherit);

            if (attributes.Any())
            {
                return (T)attributes[0];
            }
            else
            {
                return null;
            }
        }
    }
}
