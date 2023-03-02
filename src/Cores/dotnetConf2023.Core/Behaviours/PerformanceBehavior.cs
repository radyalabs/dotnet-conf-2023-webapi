using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace dotnetConf2023.Core.Behaviours;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IMessage
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async ValueTask<TResponse> Handle(TRequest message, CancellationToken cancellationToken,
        MessageHandlerDelegate<TRequest, TResponse> next)
    {
        var sw = Stopwatch.StartNew();
        var response = await next(message, cancellationToken);
        sw.Stop();

        var totalMillis = Math.Round(sw.Elapsed.TotalMilliseconds, 2);

        if (!(totalMillis > 750)) return response;

        // if response exceed 0,75s write log
        var name = typeof(TRequest).Name;
        _logger.LogWarning("Command or Query of {Name}, takes {TotalMillis}ms, that is exceed base warning", name,
            totalMillis);

        return response;
    }
}