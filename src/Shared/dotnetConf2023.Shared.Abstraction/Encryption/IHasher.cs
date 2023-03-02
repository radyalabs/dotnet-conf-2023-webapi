namespace dotnetConf2023.Shared.Abstraction.Encryption;

public interface IHasher
{
    string Hash(string data);
}