namespace dotnetConf2023.Shared.Abstraction.Models;

public sealed class DropdownKeyValue
{
    public DropdownKeyValue(string key, string value)
    {
        Key = key;
        Value = value;
    }

    public string Key { get; }
    public string Value { get; }
}