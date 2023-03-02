using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Domain.Entities;
using dotnetConf2023.Domain.Services;
using dotnetConf2023.Infrastructure.Services;
using dotnetConf2023.IntegrationTests.Dependencies;
using dotnetConf2023.Persistence;
using dotnetConf2023.Shared.Abstraction.Auth;
using dotnetConf2023.Shared.Abstraction.Databases;
using dotnetConf2023.Shared.Abstraction.Encryption;
using dotnetConf2023.Shared.Abstraction.Time;
using dotnetConf2023.Shared.Infrastructure.Auth;
using dotnetConf2023.Shared.Infrastructure.Storage;
using dotnetConf2023.UnitTests.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetConf2023.IntegrationTests.Fixtures;

public class ServiceFixture : IDisposable
{
    private readonly SqlServerDbContext _db;
    public ServiceProvider ServiceProvider { get; }

    public ServiceFixture()
    {
        var options = new DbContextOptionsBuilder<SqlServerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _db = new SqlServerDbContext(options, ContextBuilderExtensions.Create().Object,
            new Clock());

        var appInit =
            new DomainInitializer(_db, new PasswordHasher<User>(), new Clock());
        appInit.ExecuteAsync(CancellationToken.None).GetAwaiter().GetResult();

        var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
        services.AddDbContext<SqlServerDbContext>(e =>
            e.UseInMemoryDatabase(Guid.NewGuid().ToString()));
        services.AddScoped<IDbContext>(serviceProvider => serviceProvider.GetRequiredService<SqlServerDbContext>());
        services.AddSingleton(_db);
        services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserService, UserService>();
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<IAuthManager, AuthManager>();
        services
            .AddSingleton<ISecurityProvider, SecurityProvider>()
            .AddSingleton<IEncryptor, Encryptor>()
            .AddSingleton<IHasher, Hasher>()
            .AddSingleton<IMd5, Md5>()
            .AddSingleton<IRng, Rng>();
        services.AddMemoryRequestStorage();
        services.AddSingleton(new AuthOptions
            { Expiry = new TimeSpan(7, 0, 0), RefreshTokenExpiry = new TimeSpan(7, 0, 0, 0) });

        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
        _db.Dispose();
    }
}