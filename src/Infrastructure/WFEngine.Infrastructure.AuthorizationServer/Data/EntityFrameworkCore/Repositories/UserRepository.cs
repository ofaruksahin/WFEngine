using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;
using WFEngine.Domain.Common.Enums;
using WFEngine.Domain.Common.ValueObjects;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.IoC.Attributes;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore.Repositories
{
    [Inject(ServiceLifetime.Scoped)]
    public class UserRepository : RepositoryBase<AuthorizationPersistedGrantDbContext,User>, IUserRepository
    {
        public UserRepository(AuthorizationPersistedGrantDbContext context) 
            : base(context)
        {
        }

        public async Task<User> GetUserWithEmailAndPassword(string email, string password)
        {
            var query = new RepositoryExpressions<User>()
                .AddInclude(query =>
                    query
                        .Include(user => user.Tenants)
                        .ThenInclude(tenantUser => tenantUser.Tenant))
                .AddFilter(query =>
                    query.Where(
                        user =>
                            user.Email == email &&
                            user.Password == password &&
                            user.StatusId == (int)EnumStatus.Active));

            return await GetQuery(query).FirstOrDefaultAsync();
        }
    }
}
