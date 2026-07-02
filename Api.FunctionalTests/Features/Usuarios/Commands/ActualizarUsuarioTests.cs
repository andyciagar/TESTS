using Api.Application.Features.Usuarios.UpdateUsuario;
using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

public sealed class ActualizarUsuarioTests : FunctionalTestBase
{
    [Test]
    public async Task Should_update_usuario_using_http_endpoint()
    {
        var created = await Context.SendAsync(UsuarioDataFactory.CreateCommand());
        var request = new { nombre = "Carlos", apellido = "Nuñez", email = "carlos.nunez@example.com" };

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{created.Id}", request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updated = await response.Content.ReadFromJsonAsync<UpdateUsuarioResult>();
        updated.ShouldNotBeNull();
        updated.Nombre.ShouldBe("Carlos");
        updated.Apellido.ShouldBe("Nuñez");
        updated.Email.ShouldBe("carlos.nunez@example.com");
    }

    [Test]
    public async Task Should_return_not_found_when_updating_unknown_usuario()
    {
        var request = new { nombre = "Carlos", apellido = "Nuñez", email = "carlos.nunez@example.com" };

        var response = await Client.PutAsJsonAsync($"/api/usuarios/{Guid.NewGuid()}", request);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}