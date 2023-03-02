namespace dotnetConf2023.Shared.Abstraction.Storage;

public interface IRequestStorage
{
    void Set<T>(string key, T value, TimeSpan? duration = null);
    T? Get<T>(string key);
    void Remove(string key);
}