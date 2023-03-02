using dotnetConf2023.Shared.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnetConf2023.Domain.Entities;

public class UserToken : BaseEntity
{
    public UserToken()
    {
        UserTokenId = Guid.NewGuid();
        IsUsed = false;
        DeviceType = DeviceType.Android;
    }

    /// <summary>
    /// Primary key of object
    /// </summary>
    public Guid UserTokenId { get; set; }

    /// <summary>
    /// Foreign key to user object
    /// </summary>
    public Guid UserId { get; set; }

    public User? User { get; set; }

    public string ClientId { get; set; } = default!;

    /// <summary>
    /// Key to use refresh token scenario
    /// </summary>
    public string RefreshToken { get; set; } = default!;

    /// <summary>
    /// Expiration of refresh token key
    /// </summary>
    public DateTime ExpiryAt { get; set; }

    /// <summary>
    /// Flag that will use to identify refresh token key is already used
    /// </summary>
    public bool IsUsed { get; set; }

    /// <summary>
    /// When that refresh token key successfully used
    /// </summary>
    public DateTime? UsedAt { get; set; }

    /// <summary>
    /// Save device type when log in or refresh token scenario.
    /// </summary>
    public DeviceType DeviceType { get; set; }

    /// <summary>
    /// Update IsUsed and UsedAt properties.
    /// </summary>
    /// <param name="dt">DateTime</param>
    public void UseUserToken(DateTime dt)
    {
        IsUsed = true;
        UsedAt = dt;
    }
}

public sealed class UserTokenConfiguration : BaseEntityConfiguration<UserToken>
{
    protected override void EntityConfiguration(EntityTypeBuilder<UserToken> builder)
    {
        builder.HasKey(e => e.UserTokenId);
        builder.Property(e => e.UserTokenId).ValueGeneratedNever();

        builder.Property(e => e.ClientId).HasMaxLength(256);

        builder.Property(e => e.RefreshToken).HasMaxLength(256);
    }
}