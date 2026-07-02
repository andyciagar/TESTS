using Api.Application.Common.Behaviors;
using Mediator;

namespace Api.FunctionalTests.Pipeline;

public sealed class TimingBehaviorTests
{
    [Test]
    public async Task Should_log_elapsed_time_after_request_execution()
    {
        var logger = new TestLogger<TimingBehavior<TestMessage, string>>();
        var behavior = new TimingBehavior<TestMessage, string>(logger);

        var response = await behavior.Handle(new TestMessage(), async (_, _) =>
        {
            await Task.Yield();
            return "ok";
        }, CancellationToken.None);

        response.ShouldBe("ok");
        logger.Messages.Count.ShouldBe(1);
        logger.Messages[0].ShouldContain("Handled TestMessage in");
    }

    public sealed record TestMessage : IRequest<string>;
}