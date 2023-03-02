using System.Text.Encodings.Web;
using dotnetConf2023.Shared.Abstraction.Encryption;

namespace dotnetConf2023.IntegrationTests.Dependencies;

internal class SecurityProvider : ISecurityProvider
{
    private readonly IEncryptor _encryptor;
    private readonly IHasher _hasher;
    private readonly IRng _rng;
    private readonly UrlEncoder _urlEncoder;
    private readonly SecurityOptions _securityOptions;
    private readonly string _key;

    public SecurityProvider(IEncryptor encryptor, IHasher hasher,
        IRng rng, UrlEncoder urlEncoder)
    {
        _encryptor = encryptor;
        _hasher = hasher;
        _rng = rng;
        _urlEncoder = urlEncoder;
        _securityOptions = new SecurityOptions
        {
            Encryption = new EncryptionOptions
            {
                Key = "abcdefghijklmnopqrstuvwxyz"
            }
        };
        _key = _securityOptions.Encryption.Key;
    }

    public string Encrypt(string data)
        => _securityOptions.Encryption.Enabled ? _encryptor.Encrypt(data, _key) : data;

    public string Decrypt(string data)
        => _securityOptions.Encryption.Enabled ? _encryptor.Decrypt(data, _key) : data;

    public string Hash(string data) => _hasher.Hash(data);

    public string Rng(int length, bool removeSpecialChars = true) => _rng.Generate(length, removeSpecialChars);

    public string Sanitize(string value) => _urlEncoder.Encode(value);
}