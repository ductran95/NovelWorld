using System;

namespace NovelWorld.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static object ChangeType(this object @object, Type type)
        {
            object result = null;
            var nonNullType = Nullable.GetUnderlyingType(type);

            if (nonNullType != null)
            {
                type = nonNullType;
            }

            if (type.IsConvertible())
            {
                result = Convert.ChangeType(@object, type);
            }
            else
            {
                if (type.IsEnum)
                {
                    result = Convert.ChangeType(@object, typeof(int));
                    result = Enum.ToObject(type, result);
                }
            }
            
            return result;
        }

        public static object ToNullable(this object @object)
        {
            var type = @object.GetType();
            
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    return (bool?) @object;
                
                case TypeCode.Byte:
                    return (byte?) @object;
                
                case TypeCode.SByte:
                    return (sbyte?) @object;
                
                case TypeCode.Int16:
                    return (short?) @object;
                
                case TypeCode.UInt16:
                    return (ushort?) @object;
                
                case TypeCode.Int32:
                    return (int?) @object;
                
                case TypeCode.UInt32:
                    return (uint?) @object;
                
                case TypeCode.Int64:
                    return (long?) @object;
                
                case TypeCode.UInt64:
                    return (ulong?) @object;
                
                case TypeCode.Single:
                    return (float?) @object;
                
                case TypeCode.Double:
                    return (double?) @object;
                
                case TypeCode.Decimal:
                    return (decimal?) @object;
                
                case TypeCode.DateTime:
                    return (DateTime?) @object;
                
                case TypeCode.String:
                    return @object;
                
                default:
                    if (type == typeof(Guid))
                    {
                        return (bool?) @object;
                    }
                    
                    throw new NotImplementedException();
            }
        }
    }
}