namespace dotnetConf2023.Shared.Abstraction.Encryption;

public interface IEncryptor
{
    string Encrypt(string data, string key);
    string Decrypt(string data, string key);
}