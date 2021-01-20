using System;
using System.Collections.Generic;
using System.Linq;

namespace NovelWorld.Utility.Extensions
{
    public static class ListExtensions
    {
        public static object ChangeType(this List<object> list, Type type)
        {
            var nonNullType = Nullable.GetUnderlyingType(type);

            if (nonNullType != null)
            {
                type = nonNullType;
            }
            
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    if (nonNullType != null)
                    {
                        return list.Cast<bool?>().ToList();
                    }
                    return list.Cast<bool>().ToList();
                
                case TypeCode.Byte:
                    if (nonNullType != null)
                    {
                        return list.Cast<byte?>().ToList();
                    }
                    return list.Cast<byte>().ToList();
                
                case TypeCode.SByte:
                    if (nonNullType != null)
                    {
                        return list.Cast<sbyte?>().ToList();
                    }
                    return list.Cast<sbyte>().ToList();
                
                case TypeCode.Int16:
                    if (nonNullType != null)
                    {
                        return list.Cast<short?>().ToList();
                    }
                    return list.Cast<short>().ToList();
                
                case TypeCode.UInt16:
                    if (nonNullType != null)
                    {
                        return list.Cast<ushort?>().ToList();
                    }
                    return list.Cast<ushort>().ToList();
                
                case TypeCode.Int32:
                    if (nonNullType != null)
                    {
                        return list.Cast<int?>().ToList();
                    }
                    return list.Cast<int>().ToList();
                
                case TypeCode.UInt32:
                    if (nonNullType != null)
                    {
                        return list.Cast<uint?>().ToList();
                    }
                    return list.Cast<uint>().ToList();
                
                case TypeCode.Int64:
                    if (nonNullType != null)
                    {
                        return list.Cast<long?>().ToList();
                    }
                    return list.Cast<long>().ToList();
                
                case TypeCode.UInt64:
                    if (nonNullType != null)
                    {
                        return list.Cast<ulong?>().ToList();
                    }
                    return list.Cast<ulong>().ToList();
                
                case TypeCode.Single:
                    if (nonNullType != null)
                    {
                        return list.Cast<float?>().ToList();
                    }
                    return list.Cast<float>().ToList();
                
                case TypeCode.Double:
                    if (nonNullType != null)
                    {
                        return list.Cast<double?>().ToList();
                    }
                    return list.Cast<double>().ToList();
                
                case TypeCode.Decimal:
                    if (nonNullType != null)
                    {
                        return list.Cast<decimal?>().ToList();
                    }
                    return list.Cast<decimal>().ToList();
                
                case TypeCode.DateTime:
                    if (nonNullType != null)
                    {
                        return list.Cast<DateTime?>().ToList();
                    }
                    return list.Cast<DateTime>().ToList();
                
                case TypeCode.String:
                    return list.Cast<bool>().ToList();
                
                default:
                    if (type == typeof(Guid))
                    {
                        if (nonNullType != null)
                        {
                            return list.Cast<Guid?>().ToList();
                        }
                        return list.Cast<Guid>().ToList();
                    }
                    
                    throw new NotImplementedException();
            }
        }
    }
}