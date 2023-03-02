using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using dotnetConf2023.Core.Behaviours;
using Microsoft.AspNetCore.Identity;

[assembly: InternalsVisibleTo("dotnetConf2023.FunctionalTests")]
[assembly: InternalsVisibleTo("dotnetConf2023.IntegrationTests")]
[assembly: InternalsVisibleTo("dotnetConf2023.UnitTests")]

namespace dotnetConf2023.Core;

public static class Extensions
{
    public static Dictionary<string, IEnumerable<string>> GenerateCustomClaims(User user,
        DeviceType deviceType)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { user.UserId.ToString() },
            ["usr"] = new[] { user.Username },
            ["deviceType"] = new[] { deviceType.ToString() }
        };

        foreach (var userRole in user.UserRoles)
            claims.Add(ClaimTypes.Role, new[] { userRole.RoleId });

        return claims;
    }

    public static Dictionary<string, IEnumerable<string>> GenerateCustomClaims(string userId,
        string username,
        DeviceType deviceType,
        IEnumerable<string> roles)
    {
        var claims = new Dictionary<string, IEnumerable<string>>
        {
            ["xid"] = new[] { userId },
            ["usr"] = new[] { username },
            ["deviceType"] = new[] { deviceType.ToString() }
        };

        foreach (var role in roles)
            claims.Add(ClaimTypes.Role, new[] { role });

        return claims;
    }

    public static void AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), ServiceLifetime.Singleton);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(PerformanceBehavior<,>));
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
    }
}