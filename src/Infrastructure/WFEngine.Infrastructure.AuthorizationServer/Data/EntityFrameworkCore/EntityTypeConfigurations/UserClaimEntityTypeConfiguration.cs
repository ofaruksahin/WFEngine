using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WFEngine.Domain.Authorization.Entities;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Attributes;
using WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Configurations;

namespace WFEngine.Infrastructure.AuthorizationServer.Data.EntityFrameworkCore.EntityTypeConfigurations
{
	[DbContextEntityTypeConfiguration(typeof(AuthorizationPersistedGrantDbContext))]
    public class UserClaimEntityTypeConfiguration : BaseEntityTypeConfiguration<UserClaim>
	{
        public override void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder
                .Property(p => p.UserId)
                .IsRequired();

            builder
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.Value)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.IsAddToken)
                .IsRequired();

            builder
                .HasOne(p => p.User)
                .WithMany(p => p.Claims)
                .HasForeignKey(p => p.UserId)
                .HasPrincipalKey(p => p.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.ToTable("UserClaims");
            base.Configure(builder);
        }
    }
}

