using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Attributes;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Configurations;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore.EntityTypeConfigurations
{
    [DbContextEntityTypeConfiguration(typeof(AuthorizationPersistedGrantDbContext))]
    public class TenantEntityTypeConfiguration : BaseEntityTypeConfiguration<Tenant>
    {
        public override void Configure(EntityTypeBuilder<Tenant> builder)
        {
            builder
                .Property(p => p.TenantId)
                .HasMaxLength(16)
                .IsRequired();

            builder
                .Property(p => p.TenantName)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.Domain)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasMany(p => p.TenantUsers)
                .WithOne(p => p.Tenant)
                .HasForeignKey(p => p.TenantId)
                .HasPrincipalKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .ToTable("Tenants");

            base.Configure(builder);
        }
    }
}
