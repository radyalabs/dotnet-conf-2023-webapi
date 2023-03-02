using dotnetConf2023.Shared.Abstraction.Encryption;
using dotnetConf2023.Shared.Infrastructure.Security.Encryption;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace dotnetConf2023.Shared.Infrastructure.Security;

public static class Extensions
{
    public static void AddSecurity(this IServiceCollection services)
    {
        var securityOptions = services.GetOptions<SecurityOptions>("security");
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var logger = serviceProvider.GetRequiredService<ILogger<ISecurityProvider>>();
            logger.LogInformation("{Message}", securityOptions.Encryption.Enabled
                ? "AES-256 data encryption is enabled."
                : "Data encryption is disabled.");
        }

        if (securityOptions.Encryption.Enabled)
        {
            if (string.IsNullOrWhiteSpace(securityOptions.Encryption.Key))
            {
                throw new ArgumentException("Empty encryption key.", nameof(securityOptions.Encryption.Key));
            }

            var keyLength = securityOptions.Encryption.Key.Length;
            if (keyLength != 32)
            {
                throw new ArgumentException($"Invalid encryption key length: {keyLength} (required: 32 chars).",
                    nameof(securityOptions.Encryption.Key));
            }
        }

        services
            .AddSingleton(securityOptions)
            .AddSingleton<ISecurityProvider, SecurityProvider>()
            .AddSingleton<IEncryptor, Encryptor>()
            .AddSingleton<IHasher, Hasher>()
            .AddSingleton<ISha256, Sha256>()
            .AddSingleton<ISha512, Sha512>()
            .AddSingleton<IMd5, Md5>()
            .AddSingleton<IRng, Rng>();
    }
}