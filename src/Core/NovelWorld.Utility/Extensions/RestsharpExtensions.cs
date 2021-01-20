using System;
using NovelWorld.Data.Enums;
using RestSharp;

namespace NovelWorld.Utility.Extensions
{
    public static class RestsharpExtensions
    {
        public static Method ToRestsharp(this ApiMethodEnum apiMethod)
        {
            if (Enum.TryParse<Method>(apiMethod.ToString(), out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Wrong enum value");
            }
        }
        
        public static DataFormat ToRestsharp(this ApiBodyTypeEnum apiBody)
        {
            if (Enum.TryParse<DataFormat>(apiBody.ToString(), out var result))
            {
                return result;
            }
            else
            {
                throw new ArgumentException("Wrong enum value");
            }
        }
    }
}