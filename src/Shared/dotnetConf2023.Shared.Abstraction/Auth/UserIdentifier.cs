namespace dotnetConf2023.Shared.Abstraction.Auth;

public class UserIdentifier
{
    public Guid UserId { get; set; }
    public string TokenId { get; set; } = null!;
    public string IdentifierId { get; set; } = null!;
    public DateTime LastChangePassword { get; set; }
}