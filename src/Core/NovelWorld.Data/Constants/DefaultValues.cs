using System;
using System.Reflection;

namespace NovelWorld.Data.Constants
{
    public static class DefaultValues
    {
        public static readonly BindingFlags SearchPropertyFlags = BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
        
        public static readonly Type[] ConvertibleTypes = new Type[]
        {
            typeof(DBNull),
            typeof(bool),
            typeof(char),
            typeof(byte), typeof(sbyte),
            typeof(short), typeof(ushort),
            typeof(int), typeof(uint),
            typeof(long), typeof(ulong),
            typeof(float), typeof(double), typeof(decimal),
            typeof(DateTime),
            typeof(string),
        };
    }
}