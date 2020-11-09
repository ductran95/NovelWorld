using System;
using NovelWorld.Common;

namespace NovelWorld.API.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class FallbackViewAttribute: Attribute
    {
        public string ViewName { get; }

        public FallbackViewAttribute(string viewName)
        {
            Ensure.NotNullOrEmpty(viewName, nameof(viewName));
            
            ViewName = viewName;
        }
    }
}