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
    public class UserClaimRepository : RepositoryBase<AuthorizationPersistedGrantDbContext, UserClaim>, IUserClaimRepository
    {
        public UserClaimRepository(AuthorizationPersistedGrantDbContext context) : base(context)
        {
        }

        public async Task<List<UserClaim>> GetClaims(int userId)
        {
            var query = new RepositoryExpressions<UserClaim>()
                .AddFilter(query =>
                    query.Where(claim =>
                        claim.UserId == userId &&
                        claim.StatusId == (int)EnumStatus.Active));

            return await GetQuery(query).ToListAsync();
        }
    }
}

