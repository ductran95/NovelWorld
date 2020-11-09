using System;
using System.Text.Json;

namespace NovelWorld.Common.Extensions
{
    public static class JsonExtensions
    {
        public static object ToObject(this JsonElement element, Type type)
        {
            object result = null;

            var nonNullType = Nullable.GetUnderlyingType(type);
            if (nonNullType != null)
            {
                type = nonNullType;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    result = element.GetBoolean();
                    break;
                
                case TypeCode.Byte:
                    result = element.GetByte();
                    break;
                
                case TypeCode.SByte:
                    result = element.GetSByte();
                    break;
                
                case TypeCode.Int16:
                    result = element.GetInt16();
                    break;
                
                case TypeCode.UInt16:
                    result = element.GetUInt16();
                    break;
                
                case TypeCode.Int32:
                    result = element.GetInt32();
                    break;
                
                case TypeCode.UInt32:
                    result = element.GetUInt32();
                    break;
                
                case TypeCode.Int64:
                    result = element.GetInt64();
                    break;
                
                case TypeCode.UInt64:
                    result = element.GetUInt64();
                    break;
                
                case TypeCode.Single:
                    result = element.GetSingle();
                    break;
                
                case TypeCode.Double:
                    result = element.GetDouble();
                    break;
                
                case TypeCode.Decimal:
                    result = element.GetDecimal();
                    break;
                
                case TypeCode.DateTime:
                    result = element.GetDateTime();
                    break;
                
                case TypeCode.String:
                    result = element.GetString();
                    break;
                
                default:
                    if (type == typeof(Guid))
                    {
                        result = element.GetGuid();
                        break;
                    }
                    
                    throw new NotImplementedException();
            }

            if (nonNullType != null)
            {
                return result.ToNullable();
            }
            
            return result;
        }
    }
}