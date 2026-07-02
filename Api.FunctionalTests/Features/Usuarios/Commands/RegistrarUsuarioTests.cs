using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

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

    [Test]
    public async Task Should_persist_usuario_in_database_after_registration()
    {
        var command = UsuarioDataFactory.CreateCommand();
        var created = await Context.SendAsync(command);

        var persisted = await Context.ExecuteDbContextAsync(dbContext =>
            dbContext.Usuarios.IgnoreQueryFilters().FirstOrDefaultAsync(item => item.Id.Value == created.Id));

        persisted.ShouldNotBeNull();
        persisted.Email.Value.ShouldBe(command.Email.ToLowerInvariant());
        persisted.IsDeleted.ShouldBeFalse();
    }
}