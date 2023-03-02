namespace dotnetConf2023.Shared.Abstraction.Encryption;

public sealed class SecurityOptions
{
    public EncryptionOptions Encryption { get; set; } = default!;
}

public sealed class EncryptionOptions
{
    public bool Enabled { get; set; }
    public string Key { get; set; } = default!;
}