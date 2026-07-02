using Api.Application.Common.Behaviors;
using FluentValidation;
using Mediator;
using ValidationException = FluentValidation.ValidationException;

namespace Api.FunctionalTests.Pipeline;

public sealed class ValidationBehaviorTests
{
    [Test]
    public async Task Should_continue_when_validation_passes()
    {
        var validators = new IValidator<TestMessage>[] { new TestMessageValidator() };
        var behavior = new ValidationBehavior<TestMessage, string>(validators);

        var response = await behavior.Handle(new TestMessage("ok"), (_, _) => ValueTask.FromResult("done"), CancellationToken.None);

        response.ShouldBe("done");
    }

    [Test]
    public void Should_throw_when_validation_fails()
    {
        var validators = new IValidator<TestMessage>[] { new TestMessageValidator() };
        var behavior = new ValidationBehavior<TestMessage, string>(validators);

        Should.ThrowAsync<ValidationException>(async () =>
            await behavior.Handle(new TestMessage(string.Empty), (_, _) => ValueTask.FromResult("done"), CancellationToken.None));
    }

    public sealed record TestMessage(string Value) : IRequest<string>;

    public sealed class TestMessageValidator : AbstractValidator<TestMessage>
    {
        public TestMessageValidator()
        {
            RuleFor(message => message.Value)
                .NotEmpty();
        }
    }
}