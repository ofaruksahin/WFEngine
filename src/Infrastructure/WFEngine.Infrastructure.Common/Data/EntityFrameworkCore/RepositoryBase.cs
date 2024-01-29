using Microsoft.EntityFrameworkCore;
using WFEngine.Domain.Common;
using WFEngine.Domain.Common.Contracts;
using WFEngine.Domain.Common.Enums;
using WFEngine.Domain.Common.ValueObjects;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Extensions;

namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore
{
    public class RepositoryBase<TDbContext,TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : BaseEntity
    {
        private readonly TDbContext _context;

        public RepositoryBase(TDbContext context)
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

        public async Task<TEntity> Get(RepositoryExpressions<TEntity> expression)
        {
            return await GetQuery(expression).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> GetAll(RepositoryExpressions<TEntity> expression)
        {
            return await GetQuery(expression).ToListAsync();
        }

        protected IQueryable<TEntity> GetQuery(RepositoryExpressions<TEntity> expression)
        {
            var query = _context.Set<TEntity>().AsQueryable<TEntity>();

            if (expression.Filter is not null)
                query = expression.Filter(query);

            if (expression.Include is not null)
                query = expression.Include(query);

            if (expression.OrderBy is not null)
                query = expression.OrderBy(query);

            foreach (var condition in expression.ConditionalFilters)
                query = query.WhereIf(condition.Key, condition.Value);

            query = query.AsNoTracking();

            return query;
        }
    }
}
