namespace dotnetConf2023.Shared.Infrastructure.Cache;

internal sealed class RedisOptions
{
    public RedisOptions()
    {
        ConnectionString = string.Empty;
    }

    public string ConnectionString { get; set; }
}