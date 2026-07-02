using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

public sealed class ActualizarUsuarioEdgeCasesTests : FunctionalTestBase
{
    [Test]
    public async Task Should_return_bad_request_when_update_id_is_empty()
    {
        var request = new { nombre = "Carlos", apellido = "Nuñez", email = "carlos.nunez@example.com" };

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{Guid.Empty}", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_return_bad_request_when_update_email_is_invalid()
    {
        var created = await Context.SendAsync(new RegistrarUsuarioCommand("Ana", "Lopez", "ana.lopez@example.com"));
        var request = new { nombre = "Ana", apellido = "Lopez", email = "correo-invalido" };

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{created.Id}", request);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_not_allow_duplicate_email_on_update()
    {
        var first = await Context.SendAsync(new RegistrarUsuarioCommand("Ana", "Lopez", "ana.lopez@example.com"));
        _ = await Context.SendAsync(new RegistrarUsuarioCommand("Beto", "Gomez", "beto.gomez@example.com"));
        var request = new { nombre = "Ana", apellido = "Lopez", email = "beto.gomez@example.com" };

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{first.Id}", request);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}