namespace dotnetConf2023.Core.Identity.Commands.RefreshToken;

public sealed record RefreshTokenCommand : ICommand<Result<JsonWebToken>>
{
    public string ClientId { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
}