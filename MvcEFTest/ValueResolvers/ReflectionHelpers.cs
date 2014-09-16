using System;
using System.Linq;
using System.Linq.Expressions;

namespace MvcEFTest.ValueResolvers
{
    public static class ReflectionHelpers
    {
        public static string GetPropertyPath<TObj, TRet>(this TObj obj, Expression<Func<TObj, TRet>> expr)
        {
            return ExpressionOperator.GetPropertyPath(expr);
        }

        public static object GetPropertyValue(this object obj, string propertyPath)
        {
            object propertyValue = null;
            if (propertyPath.IndexOf(".", StringComparison.Ordinal) < 0)
            {
                var objType = obj.GetType();
                propertyValue = objType.GetProperty(propertyPath).GetValue(obj, null);
                return propertyValue;
            }

            var properties = propertyPath.Split('.').ToList();
            var midPropertyValue = obj;
            while (properties.Count > 0)
            {
                var propertyName = properties.First();
                properties.Remove(propertyName);
                propertyValue = midPropertyValue.GetPropertyValue(propertyName);
                midPropertyValue = propertyValue;
            }

            return propertyValue;
        }

        public static TRet GetPropertyValue<TObj, TRet>(
            this TObj obj,
            Expression<Func<TObj, TRet>> expression,
            bool silent = false)
        {
            var propertyPath = ExpressionOperator.GetPropertyPath(expression);
            var objType = obj.GetType();
            var propertyValue = objType.GetProperty(propertyPath).GetValue(obj, null);
            return (TRet)propertyValue;
        }
    }
}