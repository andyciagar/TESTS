using System.Diagnostics;
using Mediator;

namespace Api.Application.Common.Behaviors;

public sealed class TimingBehavior<TMessage, TResponse>(ILogger<TimingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        var response = await next(message, cancellationToken);

        stopwatch.Stop();
        logger.LogInformation("Handled {MessageName} in {ElapsedMilliseconds}ms", typeof(TMessage).Name, stopwatch.ElapsedMilliseconds);

        return response;
    }
}