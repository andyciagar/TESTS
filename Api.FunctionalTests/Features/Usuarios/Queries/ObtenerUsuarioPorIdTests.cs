using Api.Application.Features.Usuarios.GetUsuarioById;
using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios.Queries;

public sealed class ObtenerUsuarioPorIdTests : FunctionalTestBase
{
    [Test]
    public async Task Should_get_usuario_by_id_using_http_endpoint()
    {
        var command = UsuarioDataFactory.CreateCommand();
        var createdUsuario = await Context.SendAsync(command);

        var response = await Client.GetAsync($"/api/usuarios/{createdUsuario.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var usuario = await response.Content.ReadFromJsonAsync<GetUsuarioByIdResult>();

        usuario.ShouldNotBeNull();
        usuario.Id.ShouldBe(createdUsuario.Id);
        usuario.Nombre.ShouldBe(command.Nombre);
        usuario.Apellido.ShouldBe(command.Apellido);
        usuario.Email.ShouldBe(command.Email.ToLowerInvariant());
    }

    [Test]
    public async Task Should_get_usuario_by_id_using_mediator_pipeline()
    {
        var command = UsuarioDataFactory.CreateCommand();
        var createdUsuario = await Context.SendAsync(command);

        var usuario = await Context.SendAsync(new GetUsuarioByIdQuery(createdUsuario.Id));

        usuario.ShouldNotBeNull();
        usuario.Id.ShouldBe(createdUsuario.Id);
    }
}