using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;

namespace MvcEFTest.ValueResolvers
{
    public static class ExpressionOperator
    {
        public static string GetPropertyPath(Expression expr)
        {
            var path = new StringBuilder();
            MemberExpression memberExpression = GetMemberExpression(expr);
            do
            {
                if (path.Length > 0)
                {
                    path.Insert(0, ".");
                }

                path.Insert(0, memberExpression.Member.Name);
                memberExpression = GetMemberExpression(memberExpression.Expression);
            }
            while (memberExpression != null);
            return path.ToString();
        }

        public static MemberExpression GetMemberExpression(Expression expression)
        {
            var memberExpression = expression as MemberExpression;
            if (memberExpression != null)
            {
                return memberExpression;
            }

            if (!(expression is LambdaExpression))
            {
                return null;
            }

            var lambdaExpression = expression as LambdaExpression;
            var bodyMemberExpression = lambdaExpression.Body as MemberExpression;
            if (bodyMemberExpression != null)
            {
                return bodyMemberExpression;
            }

            var bodyUnaryExpression = lambdaExpression.Body as UnaryExpression;
            if (bodyUnaryExpression != null)
            {
                return (MemberExpression)bodyUnaryExpression.Operand;
            }

            return null;
        }
    }
}