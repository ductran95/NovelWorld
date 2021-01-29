using System;
using System.Collections.Generic;
using System.Linq;

namespace NovelWorld.Utility
{
    public static class Check
    {
        public static void Null<T>(T data, string name = "data")
        {
            if (data != null)
            {
                throw new ArgumentException($"{name} is not null");
            }
        }
        
        public static void NotNull<T>(T data, string name = "data")
        {
            if (data == null)
            {
                throw new ArgumentException($"{name} is null");
            }
        }
        
        public static void NullOrEmpty<T>(T data, string name = "data")
        {
            if (data != null && !data.Equals(default(T)))
            {
                throw new ArgumentException($"{name} is not null or empty");
            }
        }
        
        public static void NotNullOrEmpty<T>(T data, string name = "data")
        {
            if (data == null || data.Equals(default(T)))
            {
                throw new ArgumentException($"{name} is null or empty");
            }
        }
        
        public static void NotContainNull<T>(IEnumerable<T> data, string name = "data")
        {
            NotNull(data, name);
            
            // ReSharper disable once ParameterOnlyUsedForPreconditionCheck.Local
            if (data.Any(x=>x == null))
            {
                throw new ArgumentException($"{name} contain null");
            }
        }
    }
}