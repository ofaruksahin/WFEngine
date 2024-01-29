using Microsoft.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore
{
    public class AuthorizationConfigurationDbContext : BaseDbContext
    {
        public AuthorizationConfigurationDbContext(DbContextOptions<AuthorizationConfigurationDbContext> options) : base (options)
        {
        }
    }
}
