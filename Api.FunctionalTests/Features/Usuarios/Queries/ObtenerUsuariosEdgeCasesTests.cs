using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests.Features.Usuarios.Queries;

public sealed class ObtenerUsuariosEdgeCasesTests : FunctionalTestBase
{
    [TestCase(0, 10)]
    [TestCase(1, 0)]
    [TestCase(1, 101)]
    public async Task Should_return_bad_request_for_invalid_pagination(int pageNumber, int pageSize)
    {
        var response = await Client.GetAsync($"/api/usuarios?pageNumber={pageNumber}&pageSize={pageSize}");

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}