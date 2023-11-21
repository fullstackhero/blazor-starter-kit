using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CleanBlazor.Application.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ILogger<TRequest> _logger;

    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling...");
        TResponse response;

        var startTime = Stopwatch.GetTimestamp();
        try
        {
            try
            {
                _logger.LogDebug("Request properties: {@Request}", request);
            }
            catch (Exception ex)
            {
                _logger.LogDebug(ex, "Unable to serialize request properties");
            }

            response = await next();
        }
        finally
        {
            var elapsedMilliseconds = Stopwatch.GetElapsedTime(startTime).TotalMilliseconds;
            _logger.LogInformation("Handled in {ElapsedMilliseconds}ms", elapsedMilliseconds);
        }

        try
        {
            _logger.LogDebug("Response properties: {@Response}", response);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Unable to serialize response properties");
        }

        return response;
    }
}
