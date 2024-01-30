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
    public class UserClientRepository : RepositoryBase<AuthorizationPersistedGrantDbContext, UserClient>, IUserClientRepository
    {
        public UserClientRepository(AuthorizationPersistedGrantDbContext context)
            : base(context)
        {
        }

        public async Task<UserClient> GetClient(string clientId, string clientSecret)
        {
            var query = new RepositoryExpressions<UserClient>()
                .AddInclude(query =>
                    query
                    .Include(client =>
                        client.Tenant)
                    .Include(client =>
                        client.User))
                .AddFilter(query =>
                    query.Where(client =>
                        client.ClientId == clientId &&
                        client.ClientSecret == clientSecret &&
                        client.Tenant.StatusId == (int)EnumStatus.Active &&
                        client.User.StatusId == (int)EnumStatus.Active));

            return await GetQuery(query).FirstOrDefaultAsync();
        }
    }
}

