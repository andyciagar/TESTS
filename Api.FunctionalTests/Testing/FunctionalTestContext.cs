using Api.Tests.AppHost;
using Aspire.Hosting;
using Aspire.Hosting.Testing;

namespace Api.FunctionalTests.Testing;

public sealed class FunctionalTestContext : IAsyncDisposable
{
    private readonly DistributedApplication distributedApplication;
    private readonly ApiWebApplicationFactory webApplicationFactory;
    private readonly string connectionString;
    private readonly Respawner respawner;

    private FunctionalTestContext(
        DistributedApplication distributedApplication,
        ApiWebApplicationFactory webApplicationFactory,
        string connectionString,
        Respawner respawner)
    {
        this.distributedApplication = distributedApplication;
        this.webApplicationFactory = webApplicationFactory;
        this.connectionString = connectionString;
        this.respawner = respawner;
    }

    public HttpClient Client => webApplicationFactory.CreateClient();

    public IServiceProvider Services => webApplicationFactory.Services;

    public static async Task<FunctionalTestContext> CreateAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<TestAppHostMarker>();
        var distributedApplication = await builder.BuildAsync();
        await distributedApplication.StartAsync();

        var connectionString = await distributedApplication.GetConnectionStringAsync("bd")
            ?? throw new InvalidOperationException("No se pudo obtener la cadena de conexion 'bd' del AppHost de test.");

        var webApplicationFactory = new ApiWebApplicationFactory(connectionString);

        _ = webApplicationFactory.CreateClient();

        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();

        var respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.SqlServer,
            SchemasToInclude = ["dbo"]
        });

        return new FunctionalTestContext(distributedApplication, webApplicationFactory, connectionString, respawner);
    }

    public async Task ResetDatabaseAsync()
    {
        await using var connection = new SqlConnection(connectionString);
        await connection.OpenAsync();
        await respawner.ResetAsync(connection);
    }

    public async Task<T> SendAsync<T>(IRequest<T> request, CancellationToken cancellationToken = default)
    {
        await using var scope = Services.CreateAsyncScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        return await mediator.Send(request, cancellationToken);
    }

    public async Task SeedAsync(Func<ApplicationDbContext, Task> action)
    {
        await using var scope = Services.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await action(dbContext);
        await dbContext.SaveChangesAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await webApplicationFactory.DisposeAsync();
        await distributedApplication.DisposeAsync();
    }
}