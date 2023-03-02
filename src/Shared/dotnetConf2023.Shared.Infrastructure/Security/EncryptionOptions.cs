namespace dotnetConf2023.Shared.Infrastructure.Security;

internal sealed class EncryptionOptions
{
    public EncryptionOptions()
    {
        Key = string.Empty;
    }
    
    public bool Enabled { get; set; }
    public string Key { get; set; }
}