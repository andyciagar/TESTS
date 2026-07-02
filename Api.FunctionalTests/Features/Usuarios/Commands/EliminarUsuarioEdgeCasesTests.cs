using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

public sealed class EliminarUsuarioEdgeCasesTests : FunctionalTestBase
{
    [Test]
    public async Task Should_return_bad_request_when_delete_id_is_empty()
    {
        var response = await Client.DeleteAsync($"/api/usuarios/{Guid.Empty}");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_return_not_found_when_deleting_unknown_usuario()
    {
        var response = await Client.DeleteAsync($"/api/usuarios/{Guid.NewGuid()}");

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}