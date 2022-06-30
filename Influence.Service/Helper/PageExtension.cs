// using System.Linq.Expressions;
//
// namespace Influence.Core.Helper
// {
//     public static class IQueryableExtensions
//     {
//         /// <summary>
//         /// Adds pagination to query
//         /// </summary>
//         /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
//         /// <param name="query">A query for use</param>
//         /// <param name="index">Page Index</param>
//         /// <param name="size">Page Size for retrive</param>
//         /// <returns>Input query with the related data included.</returns>
//         public static IQueryable<TEntity> SkipTake<TEntity>(this IQueryable<TEntity> query, int index, int size)
//         {
//             return query.Skip((index - 1) * size).Take(size);
//         }
//
//         /// <summary>
//         /// Calculates total page count
//         /// </summary>
//         /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
//         /// <param name="query">A query for use</param>
//         /// <param name="size">Page Size for calculation</param>
//         /// <returns>Input query with the related data included.</returns>
//         public static int CalculatePageCount<TEntity>(this IQueryable<TEntity> query, int? size)
//         {
//             return (int)Math.Ceiling(query.Count() / (double)(size ?? query.Count()));
//         }
//         public static int CalculatePageCount<TEntity>(this IQueryable<TEntity> query, int count, int? size)
//         {
//             return (int)Math.Ceiling(count / (double)(size ?? count));
//         }
//         /// <summary>
//         /// Orders data by given values and directions
//         /// </summary>
//         /// <typeparam name="TEntity">The type of entity being queried.</typeparam>
//         /// <param name="query">A query for use</param>
//         /// <param name="ordination">Used for ordering <see cref="Orderable"/></param>
//         /// <returns>Input query with the related data included.</returns>
//         public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, ICollection<OrderableDto> ordination)
//         {
//             if (ordination != null)
//             {
//                 int count = 0;
//                 var expression = query.Expression;
//
//                 foreach (var item in ordination)
//                 {
//                     var property = query.ElementType.GetProperty(item.Value);
//                     if (property == null) continue;
//
//                     var parameter = Expression.Parameter(typeof(TEntity), "x");
//                     var selector = Expression.PropertyOrField(parameter, item.Value);
//
//                     if (item.IsAscending)
//                     {
//                         expression = Expression.Call(typeof(Queryable), "OrderBy",
//                            new Type[] { query.ElementType, selector.Type },
//                            expression, Expression.Quote(Expression.Lambda(selector, parameter)));
//                         count++;
//
//                     }
//                     else
//                     {
//                         expression = Expression.Call(typeof(Queryable), "OrderByDescending",
//                            new Type[] { query.ElementType, selector.Type },
//                            expression, Expression.Quote(Expression.Lambda(selector, parameter)));
//                         count++;
//                     }
//
//                 }
//                 return count > 0 ? query.Provider.CreateQuery<TEntity>(expression) : query;
//             }
//
//             return query;
//
//         }
//     }
// }
