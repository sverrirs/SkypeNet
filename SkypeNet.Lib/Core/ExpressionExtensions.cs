using System;
using System.Linq.Expressions;

namespace SkypeNet.Lib.Core
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Returns the property name for a simple property expression such as ()=> Property1
        /// </summary>
        /// <typeparam name="TPropertyType"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetPropertyName<TPropertyType>(this Expression<Func<TPropertyType>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new ArgumentException("Value must be a lamda expression", "expression");
            if (!(expression.Body is MemberExpression))
                throw new ArgumentException("The body of the expression must be a memberref", "expression");

            var m = (MemberExpression) expression.Body;
            return m.Member.Name;
        }
    }
}