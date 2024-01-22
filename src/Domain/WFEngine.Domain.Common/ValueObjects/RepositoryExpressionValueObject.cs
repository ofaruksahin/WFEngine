namespace WFEngine.Domain.Common.ValueObjects
{
    public class RepositoryExpressionValueObject<TEntity>
        where TEntity : BaseEntity
    {
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Filter { get; set; }
        public Func<IQueryable<TEntity>, IQueryable<TEntity>> Include { get; set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderBy { get; set; }

        public RepositoryExpressionValueObject()
        {
            
        }
    }
}
