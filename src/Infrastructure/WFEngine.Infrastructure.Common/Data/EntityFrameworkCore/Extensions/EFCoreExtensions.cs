using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Enums;
using WFEngine.Domain.Common.ValueObjects;

namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Extensions
{
    internal static class EFCoreExtensions
    {
        #region PaginationExtensions
        public static IQueryable<TSource> WhereIf<TSource>(
          this IQueryable<TSource> source,
          bool condition,
          Expression<Func<TSource, bool>> predicate)
        {
            if (condition)
                return source = source.Where(predicate);
            return source;
        }

        public static async Task<PaginatedModel<TEntity>> PaginateAsync<TEntity>(
            this IQueryable<TEntity> query,
            PaginationFilter paginationFilter) where TEntity : class
        {
            var totalRecords = await query.CountAsync();

            List<TEntity> paginatedData = null;

            if (paginationFilter.PageSize > -1)
            {
                query = query
                    .Skip((paginationFilter.PageNumber - 1) * paginationFilter.PageSize)
                    .Take(paginationFilter.PageSize);
            }

            var totalPages = ((double)totalRecords / (double)paginationFilter.PageSize);
            var roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            if (!string.IsNullOrEmpty(paginationFilter.SortColumn))
            {
                IOrderedQueryable<TEntity> orderedQuery = null;

                if (paginationFilter.SortDirection == EnumSortDirection.Ascending)
                    orderedQuery = query.OrderBy(paginationFilter.SortColumn);
                else
                    orderedQuery = query.OrderByDescending(paginationFilter.SortColumn);

                paginatedData = await orderedQuery.ToListAsync();
            }
            else
            {
                paginatedData = await query.ToListAsync();
            }

            return new PaginatedModel<TEntity>(paginationFilter.PageNumber, paginationFilter.PageSize, roundedTotalPages, totalRecords, paginatedData);
        }

        private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(ToLambda<T>(propertyName));
        }

        private static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(ToLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> ToLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propAsObject = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
        }
        #endregion
    }
}
