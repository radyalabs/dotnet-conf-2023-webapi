using dotnetConf2023.Shared.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnetConf2023.Domain.Entities;

public class UserRoleLog : BaseEntity
{
    public UserRoleLog()
    {
        UserRoleLogId = Guid.NewGuid();
        Type = UserRoleLogType.Given;
    }

    /// <summary>
    /// Primary key of object
    /// </summary>
    public Guid UserRoleLogId { get; set; }

    /// <summary>
    /// Foreign key to User object
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Foreign key to Role object
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Default value is UserRoleLogType.Given
    /// </summary>
    public UserRoleLogType Type { get; set; }
}

public sealed class UserRoleLogConfiguration : BaseEntityConfiguration<UserRoleLog>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserRoleLog> builder)
    {
        builder.HasKey(e => e.UserRoleLogId);
        builder.Property(e => e.UserRoleLogId).ValueGeneratedNever();
    }
}