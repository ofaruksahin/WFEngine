using Microsoft.EntityFrameworkCore;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore
{
    public class AuthorizationConfigurationDbContext : DbContext
    {
        public AuthorizationConfigurationDbContext(DbContextOptions<AuthorizationConfigurationDbContext> options) : base(options)
        {
            
        }
    }
}
