using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace dotnetConf2023.Shared.Infrastructure.Swaggers;

public static class Extensions
{
    public static void AddSwaggerGen2(this IServiceCollection services)
    {
        var options = services.GetOptions<SwaggerOptions>("auth");

        services.AddSingleton(options);
        services.AddSwaggerGen(swagger =>
        {
            swagger.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString("2022-01-01")
            });

            swagger.EnableAnnotations();
            swagger.OperationFilter<AddAuthorizationHeaderOperationFilter>();
            swagger.CustomSchemaIds(x => x.FullName);
            swagger.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = options.Title,
                Version = options.Version
            });
            swagger.AddSecurityDefinition(
                name: "Bearer",
                securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description =
                        "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
        });
    }

    public static void UseSwaggerGenAndReDoc(this IApplicationBuilder app)
    {
        var serviceProvider = app.ApplicationServices;
        var docs = serviceProvider.GetRequiredService<SwaggerOptions>();

        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseReDoc(reDoc =>
        {
            reDoc.RoutePrefix = docs.RoutePrefix;
            reDoc.SpecUrl(docs.SpecUrl);
            reDoc.DocumentTitle = docs.Title;
        });
    }
}