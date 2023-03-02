using dotnetConf2023.Shared.Abstraction.Databases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace dotnetConf2023.Shared.Infrastructure.Services;

internal sealed class ApplicationInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationInitializer> _logger;

    public ApplicationInitializer(IServiceProvider serviceProvider, ILogger<ApplicationInitializer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var initializers = scope.ServiceProvider.GetServices<IInitializer>();

        foreach (var initializer in initializers)
        {
            try
            {
                _logger.LogInformation("Running the initializer: {Name}...", initializer.GetType().Name);
                await initializer.ExecuteAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{Message}", exception.Message);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}