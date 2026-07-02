namespace Api.FunctionalTests.Testing;

public abstract class FunctionalTestBase
{
    protected HttpClient Client => FunctionalTestSetUp.Current.Client;

    protected FunctionalTestContext Context => FunctionalTestSetUp.Current;

    [SetUp]
    public async Task SetUpAsync()
    {
        await Context.ResetDatabaseAsync();
    }
}