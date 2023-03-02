using dotnetConf2023.Shared.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnetConf2023.Domain.Entities;

public class UserRole : BaseEntity
{
    /// <summary>
    /// Primary key of the object
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Primary key of the object
    /// </summary>
    public string RoleId { get; set; } = null!;
}

public sealed class UserRoleConfiguration : BaseEntityConfiguration<UserRole>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRole> builder)
    {
        builder.HasKey(e => new { e.UserId, e.RoleId });
    }
}