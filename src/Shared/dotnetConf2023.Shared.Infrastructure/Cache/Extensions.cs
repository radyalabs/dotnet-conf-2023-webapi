using dotnetConf2023.Shared.Abstraction.Cache;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace dotnetConf2023.Shared.Infrastructure.Cache;

public static class Extensions
{
    public static void AddRedisCache(this IServiceCollection services)
    {
        var options = services.GetOptions<RedisOptions>("redis");
        services.AddStackExchangeRedisCache(o => o.Configuration = options.ConnectionString);
        services.AddScoped<ICache, RedisCache>();
        services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(options.ConnectionString));
        services.AddScoped(ctx => ctx.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
    }
}