using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Attributes;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Configurations;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore.EntityTypeConfigurations
{
    [DbContextEntityTypeConfiguration(typeof(AuthorizationPersistedGrantDbContext))]
    public class UserClientEntityTypeConfiguration : BaseEntityTypeConfiguration<UserClient>
	{
        public override void Configure(EntityTypeBuilder<UserClient> builder)
        {
            builder
                .Property(p => p.ClientId)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.ClientSecret)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.TenantId)
                .HasMaxLength(16)
                .IsRequired();

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Clients)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.Tenant)
                .WithMany(p => p.Clients)
                .HasForeignKey(p => p.TenantId)
                .HasPrincipalKey(p => p.TenantId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .ToTable("UserClients");

            base.Configure(builder);
        }
    }
}

