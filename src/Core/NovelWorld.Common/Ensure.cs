using System;

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
        
        public static void NotEmpty<T>(T data, string name = "data")
        {
            Ensure.NotNull(data, name);
            
            if (data.Equals(default(T)))
            {
                throw new ArgumentException($"{name} is empty");
            }
        }
    }
}