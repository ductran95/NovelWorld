using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using NovelWorld.Common.Extensions;

namespace NovelWorld.Common.Helpers
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> CreateAndExpression<T>(ParameterExpression param, Expression exp1, Expression exp2)
        {
            var expressionAnd = Expression.AndAlso(exp1, exp2);
            return Expression.Lambda<Func<T, bool>>(expressionAnd, param);
        }
        
        public static Expression<Func<T, bool>> CreateAndExpression<T>(ParameterExpression param, Expression<Func<T, bool>> exp1, Expression<Func<T, bool>> exp2)
        {
            var expressionAnd = Expression.AndAlso(exp1, exp2);
            return Expression.Lambda<Func<T, bool>>(expressionAnd, param);
        }
            
        public static Expression<Func<T, bool>> CreateEqualExpression<T>(ParameterExpression param, string propName,
            object data)
        {
            var propertyExp = Expression.Property(param, propName);
            
            var dataType = propertyExp.Type;
            Expression dataExp = Expression.Constant(data, dataType);
            
            return Expression.Lambda<Func<T, bool>>(Expression.Equal(propertyExp, dataExp), param);
        }
        
        public static Expression<Func<T, bool>> CreateGTEExpression<T>(ParameterExpression param, string propName,
            object data)
        {
            var propertyExp = Expression.Property(param, propName);
            
            var dataType = propertyExp.Type;
            Expression dataExp = Expression.Constant(data, dataType);
            
            return Expression.Lambda<Func<T, bool>>(Expression.GreaterThanOrEqual(propertyExp, dataExp), param);
        }
        
        public static Expression<Func<T, bool>> CreateLTEExpression<T>(ParameterExpression param, string propName,
            object data)
        {
            var propertyExp = Expression.Property(param, propName);
            
            var dataType = propertyExp.Type;
            Expression dataExp = Expression.Constant(data, dataType);
            
            return Expression.Lambda<Func<T, bool>>(Expression.LessThanOrEqual(propertyExp, dataExp), param);
        }
        
        public static Expression<Func<T, bool>> CreateContainExpression<T>(ParameterExpression param, string propName,
            string data)
        {
            var propertyExp = Expression.Property(param, propName);
            MethodInfo containMethod = typeof(string).GetMethod("Contains", new[] {typeof(string)});
            
            Expression dataExp = Expression.Constant(data, typeof(string));
            
            // ReSharper disable once AssignNullToNotNullAttribute
            return Expression.Lambda<Func<T, bool>>(Expression.Call(propertyExp, containMethod, dataExp), param);
        }
        
        public static Expression<Func<T, bool>> CreateContainExpression<T>(ParameterExpression param, string propName,
            ArrayList data)
        {
            var propertyExp = Expression.Property(param, propName);
            
            var dataType = propertyExp.Type;
            var listType = typeof(List<>).MakeGenericType(dataType);

            var listData = data.Cast<JsonElement>().Select(x => x.ToObject(dataType)).ToList();
            MethodInfo containMethod = listType.GetMethod("Contains", new[] {dataType});

            object listDataWithType = listData.ChangeType(dataType);
            var dataExp = Expression.Constant(listDataWithType);
            
            // ReSharper disable once AssignNullToNotNullAttribute
            return Expression.Lambda<Func<T, bool>>(Expression.Call(dataExp, containMethod, propertyExp), param);
        }
    }
}