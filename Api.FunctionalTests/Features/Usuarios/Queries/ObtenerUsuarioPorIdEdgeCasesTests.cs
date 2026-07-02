using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests.Features.Usuarios.Queries;

public sealed class ObtenerUsuarioPorIdEdgeCasesTests : FunctionalTestBase
{
    [Test]
    public async Task Should_return_not_found_for_unknown_usuario()
    {
        var response = await Client.GetAsync($"/api/usuarios/{Guid.NewGuid()}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Should_return_bad_request_for_empty_usuario_id()
    {
        var response = await Client.GetAsync($"/api/usuarios/{Guid.Empty}");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}