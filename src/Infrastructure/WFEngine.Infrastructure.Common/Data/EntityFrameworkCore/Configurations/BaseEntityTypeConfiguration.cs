using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WFEngine.Domain.Common;

namespace WFEngine.Infrastructure.Common.Data.EntityFrameworkCore.Configurations
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(p => p.Id);

            builder
                .Property(p => p.Id)
                .IsRequired();

            builder
                .Property(p => p.Created)
                .IsRequired();

            builder
                .Property(p => p.CreatedBy)
                .HasMaxLength(100)
                .IsRequired();

            builder
                .Property(p => p.LastModified);

            builder
                .Property(p => p.LastModifiedBy)
                .HasMaxLength(100);

            builder
                .Property(p => p.StatusId)
                .IsRequired();

            builder
                .Ignore(p => p.DomainEvents);

            builder
                .Ignore(p => p.Status);
        }
    }
}
