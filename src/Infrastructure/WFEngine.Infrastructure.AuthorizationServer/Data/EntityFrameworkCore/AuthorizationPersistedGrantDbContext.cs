using Microsoft.EntityFrameworkCore;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Extensions;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore
{
    public class AuthorizationPersistedGrantDbContext : BaseDbContext
    {

        public AuthorizationPersistedGrantDbContext(
            DbContextOptions<AuthorizationPersistedGrantDbContext> options)
            : base(options)
        {
        }

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TenantUser> TenantUsers { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserClient> UserClients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureModelBuilder(typeof(AuthorizationPersistedGrantDbContext));
            base.OnModelCreating(modelBuilder);
        }
    }
}
