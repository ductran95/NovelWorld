using System;
using System.Collections.Generic;
using System.Linq;

namespace NovelWorld.Common
{
    public static class Ensure
    {
        public static void NotNull<T>(T data, string name = "data")
        {
            if (data == null)
            {
                throw new ArgumentException($"{name} is null");
            }
        }
        
        public static void NotNullOrEmpty<T>(T data, string name = "data")
        {
            Ensure.NotNull(data, name);
            
            if (data.Equals(default(T)))
            {
                throw new ArgumentException($"{name} is empty");
            }
        }
        
        public static void NotContainNull<T>(IEnumerable<T> data, string name = "data")
        {
            Ensure.NotNull(data, name);
            
            if (data.Any(x=>x == null))
            {
                throw new ArgumentException($"{name} contain null");
            }
        }
    }
}