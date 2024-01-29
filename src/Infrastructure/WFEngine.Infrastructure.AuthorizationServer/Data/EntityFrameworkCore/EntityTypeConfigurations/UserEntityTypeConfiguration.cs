using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Attributes;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Configurations;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore.EntityTypeConfigurations
{
    [DbContextEntityTypeConfiguration(typeof(AuthorizationPersistedGrantDbContext))]
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(p => p.Email)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.Password)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .Property(p => p.Language)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .HasMany(p => p.Tenants)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .ToTable("Users");

            base.Configure(builder);
        }
    }
}
