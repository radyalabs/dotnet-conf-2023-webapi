using dotnetConf2023.Shared.Abstraction.Auth;

namespace dotnetConf2023.IntegrationTests.Dependencies;

internal class AuthManager : IAuthManager
{
    public JsonWebToken CreateToken(Guid userId, string clientId, string refreshToken, string idd, string? role,
        string? audience,
        IDictionary<string, IEnumerable<string>>? claims)
    {
        return new JsonWebToken();
    }
}