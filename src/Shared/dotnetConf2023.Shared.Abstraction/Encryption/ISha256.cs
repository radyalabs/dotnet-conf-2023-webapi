namespace dotnetConf2023.Shared.Abstraction.Encryption;

public interface ISha256
{
    string Hash(string data);
}