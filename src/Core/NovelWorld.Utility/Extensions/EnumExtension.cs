using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NovelWorld.Utility.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            var displayNameAttribute = enumValue.GetType().GetField(enumValue.ToString()).GetCustomAttribute<DisplayAttribute>();

            if(displayNameAttribute != null)
            {
                return displayNameAttribute.Name;
            }

            return enumValue.ToString();
        }
    }
}
