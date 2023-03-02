using System.Security.Cryptography;
using System.Text;
using dotnetConf2023.Shared.Abstraction.Encryption;

namespace dotnetConf2023.Shared.Infrastructure.Security.Encryption;

internal sealed class Hasher : IHasher
{
    public string Hash(string data)
    {
        var bytes = SHA512.HashData(Encoding.UTF8.GetBytes(data));
        var builder = new StringBuilder();
        foreach (var @byte in bytes)
        {
            builder.Append(@byte.ToString("x2"));
        }

        return builder.ToString();
    }
}