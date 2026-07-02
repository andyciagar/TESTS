using Api.Application.Behaviors;
using Mediator;

namespace Api.FunctionalTests.Pipeline;

public sealed class LoggingBehaviorTests
{
    [Test]
    public async Task Should_log_before_and_after_request_execution()
    {
        var logger = new TestLogger<LoggingBehavior<TestMessage, string>>();
        var behavior = new LoggingBehavior<TestMessage, string>(logger);

        var response = await behavior.Handle(new TestMessage(), (_, _) => ValueTask.FromResult("ok"), CancellationToken.None);

        response.ShouldBe("ok");
        logger.Messages.Count.ShouldBe(2);
        logger.Messages[0].ShouldContain("Handling TestMessage");
        logger.Messages[1].ShouldContain("Handled TestMessage");
    }

    public sealed record TestMessage : IRequest<string>;
}