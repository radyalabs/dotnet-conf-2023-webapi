namespace dotnetConf2023.Core.Models;

public record UserCompactForLoginModel
{
    public Guid UserId { get; set; }
    public string Username { get; set; } = null!;
    public DateTime? LastPasswordChangeAt { get; set; }
    public string[] Roles { get; set; } = null!;
}