namespace dotnetConf2023.Shared.Abstraction.Encryption;

public interface IRng
{
    string Generate(int length = 50, bool removeSpecialChars = true);
}