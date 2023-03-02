using System.Text;
using dotnetConf2023.Shared.Abstraction.Auth;
using dotnetConf2023.Shared.Abstraction.Storage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace dotnetConf2023.Shared.Infrastructure.Auth;

public static class Extensions
{
    private const string AccessTokenCookieName = "__access-token";
    private const string AuthorizationHeader = "authorization";

    public static void AddRequirement<T>(this IServiceCollection services) where T : class, IRequirement
    {
        services.AddSingleton<IRequirement, T>();
    }

    public static void AddAuth(this IServiceCollection services,
        Action<JwtBearerOptions>? optionsFactory)
    {
        var options = services.GetOptions<AuthOptions>("auth");
        services.AddSingleton<IAuthManager, AuthManager>();
        services.AddSingleton<RequirementManager>();

        if (options.AuthenticationDisabled)
        {
            services.AddSingleton<IPolicyEvaluator, DisabledAuthenticationPolicyEvaluator>();
        }

        var tokenValidationParameters = new TokenValidationParameters
        {
            RequireAudience = options.RequireAudience,
            ValidIssuer = options.ValidIssuer,
            ValidIssuers = options.ValidIssuers,
            ValidateActor = options.ValidateActor,
            ValidAudience = options.ValidAudience,
            ValidAudiences = options.ValidAudiences,
            ValidateAudience = options.ValidateAudience,
            ValidateIssuer = options.ValidateIssuer,
            ValidateLifetime = options.ValidateLifetime,
            ValidateTokenReplay = options.ValidateTokenReplay,
            ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
            SaveSigninToken = options.SaveSigninToken,
            RequireExpirationTime = options.RequireExpirationTime,
            RequireSignedTokens = options.RequireSignedTokens,
            ClockSkew = TimeSpan.Zero
        };

        if (string.IsNullOrWhiteSpace(options.IssuerSigningKey))
        {
            throw new ArgumentException("Missing issuer signing key.", nameof(options.IssuerSigningKey));
        }

        if (!string.IsNullOrWhiteSpace(options.AuthenticationType))
        {
            tokenValidationParameters.AuthenticationType = options.AuthenticationType;
        }

        var rawKey = Encoding.UTF8.GetBytes(options.IssuerSigningKey);
        tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);

        if (!string.IsNullOrWhiteSpace(options.NameClaimType))
        {
            tokenValidationParameters.NameClaimType = options.NameClaimType;
        }

        if (!string.IsNullOrWhiteSpace(options.RoleClaimType))
        {
            tokenValidationParameters.RoleClaimType = options.RoleClaimType;
        }

        services
            .AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.Authority = options.Authority;
                o.Audience = options.Audience;
                o.MetadataAddress = options.MetadataAddress!;
                o.SaveToken = options.SaveToken;
                o.RefreshOnIssuerKeyNotFound = options.RefreshOnIssuerKeyNotFound;
                o.RequireHttpsMetadata = options.RequireHttpsMetadata;
                o.IncludeErrorDetails = options.IncludeErrorDetails;
                o.TokenValidationParameters = tokenValidationParameters;
                if (!string.IsNullOrWhiteSpace(options.Challenge))
                {
                    o.Challenge = options.Challenge;
                }

                o.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var requestStorage = context.HttpContext.RequestServices.GetRequiredService<IRequestStorage>();

                        // get client id from claims
                        var idt = context.Principal!.Claims.First(e => e.Type == "idt");
                        var userId = context.Principal!.Claims.First(e => e.Type == "xid");
                        var idd = context.Principal!.Claims.First(e => e.Type == "idd");

                        var userIdentifier = requestStorage.Get<UserIdentifier>($"{userId.Value}{idt.Value}");
                        if (userIdentifier is null) context.Fail("Invalid");

                        if (userIdentifier is null) return Task.CompletedTask;

                        if (idd.Value != userIdentifier.TokenId) context.Fail("Invalid");

                        return Task.CompletedTask;
                    }
                };

                optionsFactory?.Invoke(o);
            });

        services.AddSingleton(options);
        if (options.Cookie is not null)
            services.AddSingleton(options.Cookie);
        services.AddSingleton(tokenValidationParameters);

        using var serviceProvider = services.BuildServiceProvider();
        var requirementManager = serviceProvider.GetRequiredService<RequirementManager>();
        requirementManager.Populate();
        services.AddAuthorization(authorization =>
        {
            if (!requirementManager.Requirements.Any()) return;
            foreach (var policy in requirementManager.Requirements)
                authorization.AddPolicy(policy.Key, x => x.RequireClaim("permissions", policy.Value));
        });
    }

    public static void UseAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.Use(async (ctx, next) =>
        {
            if (ctx.Request.Headers.ContainsKey(AuthorizationHeader))
            {
                ctx.Request.Headers.Remove(AuthorizationHeader);
            }

            if (ctx.Request.Cookies.ContainsKey(AccessTokenCookieName))
            {
                var authenticateResult = await ctx.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);
                if (authenticateResult.Succeeded)
                {
                    ctx.User = authenticateResult.Principal;
                }
            }

            await next();
        });
    }
}