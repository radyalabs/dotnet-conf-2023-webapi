namespace dotnetConf2023.Shared.Infrastructure.Security;

internal sealed class SecurityOptions
{
    public EncryptionOptions Encryption { get; set; } = null!;
}