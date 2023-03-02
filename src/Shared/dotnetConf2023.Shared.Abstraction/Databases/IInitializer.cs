namespace dotnetConf2023.Shared.Abstraction.Databases;

public interface IInitializer
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}