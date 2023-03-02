namespace dotnetConf2023.Shared.Abstraction.Encryption;

public interface ISha512
{
    string Hash(string data);
}