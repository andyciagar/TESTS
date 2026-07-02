using Mediator;

namespace Api.Application.Behaviors;

public sealed class LoggingBehavior<TMessage, TResponse>(ILogger<LoggingBehavior<TMessage, TResponse>> logger)
    : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(
        TMessage message,
        MessageHandlerDelegate<TMessage, TResponse> next,
        CancellationToken cancellationToken)
    {
        var messageName = typeof(TMessage).Name;

        logger.LogInformation("Handling {MessageName}", messageName);

        var response = await next(message, cancellationToken);

        logger.LogInformation("Handled {MessageName}", messageName);

        return response;
    }
}