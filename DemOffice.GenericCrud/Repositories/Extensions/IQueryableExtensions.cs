using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using DemOffice.GenericCrud.Repositories.Helpers;

namespace DemOffice.GenericCrud.Repositories.Extensions
{
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1305:FieldNamesMustNotUseHungarianNotation", Justification = "Reviewed.")]
    internal static class QueryableExtensions
    {
        /// <summary>This extension to IQueryable allows to apply ordering
        /// dynamically by string representation of property name</summary>
        /// <typeparam name="T">IQueryable</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="orderingType">Type of the ordering.</param>
        /// <returns>IOrderedQueryable{T}</returns>
        public static IOrderedQueryable<T> ApplyOrder<T>(this IQueryable<T> source, string propertyName, string orderingType)
        {
            var entityType = typeof(T);
            var methodName = orderingType.ToLower() == "desc"
                ? nameof(Queryable.OrderByDescending)
                : nameof(Queryable.OrderBy);
            var lambda = ExpressionHelper.GetLambdaPointToProperty<T>(propertyName, out var propertyType);

            // Getting the specified method of ordering
            // with appropriate types of entity and property in runtime
            // and invoking it on passed collection and built lambda expression
            var result = typeof(Queryable)
                .GetMethods()
                .Single(method => method.Name == methodName

                                  && method.IsGenericMethodDefinition
                                  && method.GetGenericArguments().Length == 2
                                  && method.GetParameters().Length == 2)
                .MakeGenericMethod(entityType, propertyType)
                .Invoke(null, new object[] { source, lambda });

            return (IOrderedQueryable<T>)result;
        }

        /// <summary>
        /// This extension to IQueryable allows to find entities
        /// that contain given substring in given properties
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="searchQuery">The search query.</param>
        /// <param name="properties">The properties.</param>
        /// <returns>System.Linq.IQueryable{T}.</returns>
        public static IQueryable<T> Search<T>(this IQueryable<T> source, string searchQuery, List<string> properties)
        {
            // Preparing expression parts
            var argumentExpr = Expression.Parameter(typeof(T), "e");
            var searchQueryExpr = Expression.Constant(searchQuery);
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), new[] { typeof(string) });
            BinaryExpression expression = null;

            if (properties.Count == 1)
            {
                var propertyExpr = Expression.Property(argumentExpr, properties[0]);
                var idExpression = Expression.Call(propertyExpr, containsMethod, searchQueryExpr);
                var result = Expression.Lambda<Func<T, bool>>(idExpression, argumentExpr);
                return source.Where(result);
            }

            for (var i = 0; i < properties.Count(); i++)
            {
                // Making sure that there's another property on next index
                if (i + 1 >= properties.Count())
                {
                    continue;
                }

                // Building search expressions for both current index property and next one
                var propertyExpr = Expression.Property(argumentExpr, properties[i]);
                var propertyExprNext = Expression.Property(argumentExpr, properties[i + 1]);
                var containsMethodExpr = Expression.Call(propertyExpr, containsMethod, searchQueryExpr);
                var containsMethodExprNext = Expression.Call(propertyExprNext, containsMethod, searchQueryExpr);

                // Initializing the expression or adding OR statement, using built expression as operand
                expression = Expression.OrElse((Expression)expression ?? containsMethodExpr, containsMethodExprNext);
            }

            // Building lambda from expression
            // Final expression will be a set of searches
            // (using Contains() method), joined with OR operator
            var lambda = Expression.Lambda<Func<T, bool>>(expression, argumentExpr);
            return source.Where(lambda);
        }

        /// <summary>
        ///  This extension to IQueryable allows to find entity by id.
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="id">The identifier.</param>
        /// <returns>System.Linq.IQueryable{T}.</returns>
        public static IQueryable<T> GetById<T>(this IQueryable<T> source, long id)
        {
            var argumentExpr = Expression.Parameter(typeof(T), "e");
            var idExpression = Expression.Constant(id);
            var propertyExpr = Expression.Property(argumentExpr, "Id");
            var equalsMethodExpr = Expression.Equal(idExpression, Expression.Convert(propertyExpr, typeof(long)));

            var lambda = Expression.Lambda<Func<T, bool>>(equalsMethodExpr, argumentExpr);

            return source.Where(lambda);
        }

        /// <summary>
        /// This extension to IQueryable allows to filter entities by id
        /// </summary>
        /// <typeparam name="T">The type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="ids">The ids.</param>
        /// <returns>System.Linq.IQueryable{T}.</returns>
        public static IQueryable<T> GetByIds<T>(this IQueryable<T> source, IEnumerable<long> ids)
        {
            var argumentExpr = Expression.Parameter(typeof(T), "e");
            var idExpression = Expression.Constant(ids);

            var containsMethod = typeof(Enumerable)
                .GetMethods()
                .Where(x => x.Name == nameof(Enumerable.Contains))
                .Single(x => x.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(long));

            var propertyExpr = Expression.Property(argumentExpr, "Id");
            var containsMethodExpr =
                Expression.Call(containsMethod, idExpression, Expression.Convert(propertyExpr, typeof(long)));

            var lambda = Expression.Lambda<Func<T, bool>>(containsMethodExpr, argumentExpr);

            return source.Where(lambda);
        }
    }
}
