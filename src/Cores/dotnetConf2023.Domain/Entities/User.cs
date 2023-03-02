using dotnetConf2023.Domain.ValueObjects;
using dotnetConf2023.Shared.Abstraction.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace dotnetConf2023.Domain.Entities;

public class User : BaseEntity
{
    public User()
    {
        UserId = Guid.NewGuid();
        UserState = UserState.Active;
        EmailActivationStatus = EmailActivationStatus.Skip;

        UserRoles = new HashSet<UserRole>();
        UserTokens = new HashSet<UserToken>();
    }

    /// <summary>
    /// Primary key of the object.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Username of user, most likely it is email.
    /// </summary>
    public string Username { get; set; } = null!;

    /// <summary>
    /// Store uppercase value of Username.
    /// </summary>
    public string NormalizedUsername { get; set; } = null!;

    /// <summary>
    /// Password hashed with IPasswordHasher.
    /// </summary>
    public string? Password { get; set; }

    /// <summary>
    /// last password change datetime, its update when user object first time created if password present,
    /// also changed by admin or user itself.
    /// </summary>
    public DateTime? LastPasswordChangeAt { get; set; }

    /// <summary>
    /// Value object of Email.
    /// </summary>
    public Email? Email { get; set; }

    /// <summary>
    /// Full name of user.
    /// </summary>
    public string FullName { get; set; } = null!;

    /// <summary>
    /// Default value is UserState.Active.
    /// </summary>
    public UserState UserState { get; set; }

    /// <summary>
    /// Input using national number, without 0 or + or +_country_code.
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Birth date of user.
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    /// Gender of user <see cref="UserGender"/>.
    /// </summary>
    public UserGender? Gender { get; set; }

    /// <summary>
    /// Country name saved to user.
    /// </summary>
    public string? Country { get; set; }

    /// <summary>
    /// Containing description of user.
    /// </summary>
    public string? AboutMe { get; set; }

    /// <summary>
    /// Containing country code. ex : 62 represent Indonesia.
    /// </summary>
    public int? CountryCode { get; set; }

    /// <summary>
    /// This value represent if user needs email activation for certain user case.
    ///
    /// Default value is EmailActivationStatus.Skip.
    /// </summary>
    public EmailActivationStatus EmailActivationStatus { get; set; }

    /// <summary>
    /// Contains email activation code.
    /// </summary>
    public string? EmailActivationCode { get; set; }

    /// <summary>
    /// DateTime when email has activated.
    /// </summary>
    public DateTime? EmailActivationAt { get; set; }

    public string? RequestId { get; set; }

    public ICollection<UserRole> UserRoles { get; }
    public ICollection<UserToken> UserTokens { get; }
}

public sealed class UserConfiguration : BaseEntityConfiguration<User>
{
    protected override void EntityConfiguration(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.UserId);
        builder.Property(e => e.UserId).ValueGeneratedNever();
        builder.Property(e => e.Username).HasMaxLength(256);
        builder.Property(e => e.NormalizedUsername).HasMaxLength(256);
        builder.HasIndex(e => e.NormalizedUsername);
        builder.Property(e => e.Password).HasMaxLength(1024);
        builder.Property(e => e.FullName).HasMaxLength(256);
        builder.HasIndex(e => e.FullName);
        builder.Property(e => e.PhoneNumber).HasMaxLength(256);
        builder.Property(e => e.Country).HasMaxLength(256);
        builder.Property(e => e.AboutMe).HasMaxLength(256);
        builder.Property(e => e.EmailActivationCode).HasMaxLength(1024);
        builder.Property(x => x.Email).HasMaxLength(256)
            .HasConversion(x => x!.Value, x => new Email(x));
        builder.Property(e => e.RequestId).HasMaxLength(450);
    }
}