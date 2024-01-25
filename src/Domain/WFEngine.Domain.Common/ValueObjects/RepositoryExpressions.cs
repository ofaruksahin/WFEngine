using System.Linq.Expressions;

namespace WFEngine.Domain.Common.ValueObjects
{
    public class RepositoryExpressions<TEntity>
        where TEntity : BaseEntity
    {
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Filter { get; set; }
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderByDescending { get; set; }
        public List<KeyValuePair<bool, Expression<Func<TEntity, bool>>>> ConditionalFilters { get; set; }
        public PaginationFilter PaginationFilter { get; set; }

        public RepositoryExpressions()
        {
            ConditionalFilters = new List<KeyValuePair<bool, Expression<Func<TEntity, bool>>>>();
        }
    }

    public static class RepositoryExpressionExtension
    {
        public static RepositoryExpressions<TEntity> AddFilter<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> filter)
            where TEntity : BaseEntity
        {
            @this.Filter = filter;
            return @this;
        }

        public static RepositoryExpressions<TEntity> AddInclude<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
            where TEntity : BaseEntity
        {
            @this.Include = include;
            return @this;
        }

        public static RepositoryExpressions<TEntity> AddOrderBy<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
            where TEntity : BaseEntity
        {
            @this.OrderBy = orderBy;
            return @this;
        }

        public static RepositoryExpressions<TEntity> AddOrderByDescending<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderByDescending)
            where TEntity : BaseEntity
        {
            @this.OrderByDescending = orderByDescending;
            return @this;
        }

        public static RepositoryExpressions<TEntity> AddPaginationFilter<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            PaginationFilter paginationFilter)
            where TEntity : BaseEntity
        {
            @this.PaginationFilter = paginationFilter;
            return @this;
        }

        public static RepositoryExpressions<TEntity> AddConditionalFilter<TEntity>(
            this RepositoryExpressions<TEntity> @this,
            bool condition,
            Expression<Func<TEntity, bool>> predicate)
            where TEntity : BaseEntity
        {
            @this.ConditionalFilters.Add(new KeyValuePair<bool, Expression<Func<TEntity, bool>>>(condition, predicate));
            return @this;
        }
    }
}
