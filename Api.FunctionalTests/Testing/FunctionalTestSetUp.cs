using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests;

[SetUpFixture]
public sealed class FunctionalTestSetUp
{
    public static FunctionalTestContext Current { get; private set; } = default!;

    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        Current = await FunctionalTestContext.CreateAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDownAsync()
    {
        if (Current is not null)
        {
            await Current.DisposeAsync();
        }
    }
}