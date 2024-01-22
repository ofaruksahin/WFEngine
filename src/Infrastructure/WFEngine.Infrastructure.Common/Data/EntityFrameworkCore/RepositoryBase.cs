using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Contracts;
using WFEngine.Domain.Common.Enums;
using WFEngine.Domain.Common.ValueObjects;

namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore
{
    public class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BaseDbContext _context;

        public RepositoryBase(BaseDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(TEntity entities)
        {
            await _context.Set<TEntity>().AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            entity.Status = EnumStatus.Passive;
            _context.Set<TEntity>().Update(entity);
        }

        public void DeleteRange(List<TEntity> entities)
        {
            entities.ForEach(e => e.Status = EnumStatus.Passive);
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }

        public void UpdateRange(List<TEntity> entities)
        {
            _context.Set<TEntity>().UpdateRange(entities);
        }

        public async Task<List<TEntity>> GetAll(RepositoryExpressionValueObject<TEntity> expression)
        {
            return await GetQuery(expression).ToListAsync();
        }

        private IQueryable<TEntity> GetQuery(RepositoryExpressionValueObject<TEntity> expression)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (expression.Filter is not null)
                query = expression.Filter(query);

            if (expression.Include is not null)
                query = expression.Include(query);

            if (expression.OrderBy is not null)
                query = expression.OrderBy(query);

            return query;
        }
    }
}
