using System.Runtime.CompilerServices;
using dotnetConf2023.Core;
using dotnetConf2023.Core.Abstractions;
using dotnetConf2023.Domain.Services;
using dotnetConf2023.Infrastructure.Services;
using dotnetConf2023.Persistence;
using dotnetConf2023.Shared.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("dotnetConf2023.IntegrationTests")]

namespace dotnetConf2023.Infrastructure;

public static class Extensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddCore();
        services.AddScoped<IUserService, UserService>();
        services.AddSqlServerDbContext();
        services.AddInitializer<DomainInitializer>();
    }
}