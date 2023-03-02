namespace dotnetConf2023.Shared.Infrastructure.Api;

internal sealed class CorsOptions
{
    public CorsOptions()
    {
        AllowedOrigins = new List<string>();
        AllowedMethods = new List<string>();
        AllowedHeaders = new List<string>();
        ExposedHeaders = new List<string>();
    }

    public bool AllowCredentials { get; set; }
    public List<string> AllowedOrigins { get; }
    public List<string> AllowedMethods { get; }
    public List<string> AllowedHeaders { get; }
    public List<string> ExposedHeaders { get; }
}