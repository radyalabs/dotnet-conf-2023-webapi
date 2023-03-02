using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnetConf2023.Shared.Abstraction.Entities;

public abstract class BaseEntityConfiguration<TBaseEntity> : IEntityTypeConfiguration<TBaseEntity>
    where TBaseEntity : BaseEntity
{
    public void Configure(EntityTypeBuilder<TBaseEntity> builder)
    {
        builder.Property(e => e.CreatedBy).HasMaxLength(maxLength: 256);
        builder.Property(e => e.CreatedByName).HasMaxLength(maxLength: 256);
        builder.HasIndex(e => e.CreatedDt);
        builder.HasIndex(e => e.CreatedDtServer);

        builder.Property(e => e.LastUpdatedBy).HasMaxLength(maxLength: 256);
        builder.Property(e => e.LastUpdatedByName).HasMaxLength(maxLength: 256);
        builder.HasIndex(e => e.LastUpdatedDt);
        builder.HasIndex(e => e.LastUpdatedDtServer);

        builder.Property(e => e.DeletedBy).HasMaxLength(maxLength: 256);
        builder.Property(e => e.DeletedByName).HasMaxLength(maxLength: 256);
        builder.HasIndex(e => e.DeletedByDt);

        builder.HasQueryFilter(e => e.DeletedByDt == null);

        EntityConfiguration(builder);
    }

    protected abstract void EntityConfiguration(EntityTypeBuilder<TBaseEntity> builder);
}