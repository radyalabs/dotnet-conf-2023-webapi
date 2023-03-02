using dotnetConf2023.Shared.Infrastructure.Auth;
using Moq;

namespace dotnetConf2023.UnitTests.Extensions;

public static class AuthOptionsBuilderExtensions
{
    public static AuthOptions Create()
    {
        return new AuthOptions
        {
            Expiry = TimeSpan.FromMinutes(30),
            RefreshTokenExpiry = TimeSpan.FromMinutes(30)
        };
    }
}