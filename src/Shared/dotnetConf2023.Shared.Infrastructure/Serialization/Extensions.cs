using dotnetConf2023.Shared.Abstraction.Serialization;
using dotnetConf2023.Shared.Infrastructure.Serialization.Jil;
using dotnetConf2023.Shared.Infrastructure.Serialization.SystemTextJson;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetConf2023.Shared.Infrastructure.Serialization;

public static class Extensions
{
    public static void AddDefaultJsonSerialization(this IServiceCollection services)
    {
        // this can be switch easily to another json serializer for example
        //services.AddSingleton<IJsonSerializer, JilJsonSerializer>();
        services.AddSingleton<IJsonSerializer, SystemTextJsonSerializer>();
    }
}