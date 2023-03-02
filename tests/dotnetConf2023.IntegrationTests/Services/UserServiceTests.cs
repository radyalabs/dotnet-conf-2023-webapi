using dotnetConf2023.Domain.Entities;
using dotnetConf2023.Infrastructure.Services;
using dotnetConf2023.IntegrationTests.Fixtures;
using dotnetConf2023.Persistence;
using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace dotnetConf2023.IntegrationTests.Services;

[Collection(Constant.ServiceCollectionDefaultName)]
public class UserServiceTests : IClassFixture<ServiceFixture>
{
    private readonly UserService _service;
    private readonly IDbContext _dbContext;

    public UserServiceTests(ServiceFixture fixture)
    {
        var db = fixture.ServiceProvider.GetRequiredService<SqlServerDbContext>();
        _service = new UserService(db);
        _dbContext = db;
    }

    [Fact]
    public async Task TestUserServiceWhenDataInsertedThenGetByUserIdShouldNotBeNull()
    {
        const string username = "test@test.com";

        var user = new User
        {
            Username = username,
            NormalizedUsername = username.ToUpperInvariant(),
            FullName = username
        };

        _dbContext.Insert(user);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var result = await _service.GetUserByIdAsync(user.UserId, CancellationToken.None);
        result.ShouldNotBe(null);
        result!.Username.ShouldBe(user.Username);
        result.NormalizedUsername.ShouldBe(user.NormalizedUsername);
        result.FullName.ShouldBe(user.FullName);
    }

    [Fact]
    public async Task TestUserServiceWhenDataInsertedThenGetByAnotherUserIdShouldBeNull()
    {
        const string username = "test@test.com";

        var user = new User
        {
            Username = username,
            NormalizedUsername = username.ToUpperInvariant(),
            FullName = username
        };

        _dbContext.Insert(user);
        await _dbContext.SaveChangesAsync(CancellationToken.None);

        var result = await _service.GetUserByIdAsync(Guid.NewGuid(), CancellationToken.None);
        result.ShouldBeNull();
    }

    [Fact]
    public async Task TestUserServiceGetDataAdministratorShouldNotBeNull()
    {
        var result = await _service.GetUserByIdAsync(Guid.Empty, CancellationToken.None);
        result.ShouldNotBe(null);
    }
}