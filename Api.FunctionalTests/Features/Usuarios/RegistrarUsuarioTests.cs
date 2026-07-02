using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios;

public sealed class RegistrarUsuarioTests : FunctionalTestBase
{
    [Test]
    public async Task Should_register_usuario_using_http_endpoint()
    {
        var command = UsuarioDataFactory.CreateCommand();

        var response = await Client.PostAsJsonAsync("/api/usuarios", command);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var createdUsuario = await response.Content.ReadFromJsonAsync<CreateUsuarioResult>();

        createdUsuario.ShouldNotBeNull();
        createdUsuario.Nombre.ShouldBe(command.Nombre);
        createdUsuario.Apellido.ShouldBe(command.Apellido);
        createdUsuario.Email.ShouldBe(command.Email.ToLowerInvariant());
    }

    [Test]
    public async Task Should_register_usuario_using_mediator_pipeline()
    {
        var command = UsuarioDataFactory.CreateCommand();

        var result = await Context.SendAsync(command);

        result.ShouldNotBeNull();
        result.Nombre.ShouldBe(command.Nombre);
        result.Apellido.ShouldBe(command.Apellido);
        result.Email.ShouldBe(command.Email.ToLowerInvariant());
    }
}