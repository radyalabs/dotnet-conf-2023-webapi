namespace dotnetConf2023.Shared.Abstraction.Auth;

public interface IAuthManager
{
    JsonWebToken CreateToken(Guid userId, string clientId, string refreshToken, string idd, string? role,
        string? audience,
        IDictionary<string, IEnumerable<string>>? claims);
}