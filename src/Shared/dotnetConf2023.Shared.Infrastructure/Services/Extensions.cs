using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetConf2023.Shared.Infrastructure.Services;

public static class Extensions
{
    public static void AddApplicationInitializer(this IServiceCollection services)
    {
        services.AddHostedService<ApplicationInitializer>();
    }
    
    public static void AddInitializer<T>(this IServiceCollection services) where T : class, IInitializer
    {
        services.AddTransient<IInitializer, T>();
    }
}