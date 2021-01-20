using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NovelWorld.Utility.Extensions
{
    public static class AssemblyExtensions
    {
        public static IEnumerable<Assembly> GetAssemblies(this AppDomain appDomain, string prefix)
        {
            var allAssemblies = appDomain.GetAssemblies();
            return allAssemblies.Where(x => x.GetName().Name.StartsWith(prefix));
        }
    }
}