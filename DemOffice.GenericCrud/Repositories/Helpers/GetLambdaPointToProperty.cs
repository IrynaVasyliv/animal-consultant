using System;
using System.Linq.Expressions;
using System.Reflection;

namespace DemOffice.GenericCrud.Repositories.Helpers
{
    /// <summary>
    /// Class ExpressionHelper.
    /// </summary>
    internal class ExpressionHelper
    {
        /// <summary>
        /// Gets the lambda point to property.
        /// </summary>
        /// <typeparam name="T">Property type.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyType">Type of the property.</param>
        /// <returns>LambdaExpression.</returns>
        public static LambdaExpression GetLambdaPointToProperty<T>(string propertyName, out Type propertyType)
        {
            // Building lambda expression to point out the desired property to sort on
            var entityType = typeof(T);
            var expressionParameter = Expression.Parameter(entityType, "x");
            Expression expression = expressionParameter;
            var propertyInfo = entityType.GetProperty(
                propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            expression = Expression.Property(expression, propertyInfo);
            propertyType = propertyInfo.PropertyType;
            var delegateType = typeof(Func<,>).MakeGenericType(entityType, propertyType);

            return Expression.Lambda(delegateType, expression, expressionParameter);
        }
    }
}
