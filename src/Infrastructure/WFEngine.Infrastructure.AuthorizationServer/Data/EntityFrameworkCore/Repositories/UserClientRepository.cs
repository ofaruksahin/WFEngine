using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Domain.Authorization.Repositories;
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

        public async Task<UserClient> GetClient(string clientId)
        {
            var query = new RepositoryExpressions<UserClient>()
                .AddFilter(query =>
                    query.Where(client =>
                        client.ClientId == clientId))
                .AddInclude(query =>
                    query.Include(client =>
                        client.Claims));

            return await GetQuery(query).FirstOrDefaultAsync();
        }
    }
}

