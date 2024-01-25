using WFEngine.Domain.Common.ValueObjects;

namespace WFEngine.Domain.Common.Contracts
{
    public interface IRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(TEntity entities);
        void Update(TEntity entity);
        void UpdateRange(List<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(List<TEntity> entities);
        Task<TEntity> Get(RepositoryExpressions<TEntity> expression);
        Task<List<TEntity>> GetAll(RepositoryExpressions<TEntity> expression);
    }
}
