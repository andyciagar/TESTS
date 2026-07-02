using Api.Application.Features.Usuarios.DeleteUsuario;
using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

public sealed class EliminarUsuarioTests : FunctionalTestBase
{
    [Test]
    public async Task Should_soft_delete_usuario_using_http_endpoint()
    {
        var created = await Context.SendAsync(UsuarioDataFactory.CreateCommand());

        var response = await Client.DeleteAsync($"/api/usuarios/{created.Id}");

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var deleted = await Context.ExecuteDbContextAsync(dbContext =>
            dbContext.Usuarios.IgnoreQueryFilters().FirstAsync(item => item.Id.Value == created.Id));

        deleted.IsDeleted.ShouldBeTrue();
    }

    [Test]
    public async Task Should_not_return_deleted_usuario_in_query_results()
    {
        var created = await Context.SendAsync(UsuarioDataFactory.CreateCommand());
        await Context.SendAsync(new DeleteUsuarioCommand(created.Id));

        var byIdResponse = await Client.GetAsync($"/api/usuarios/{created.Id}");
        var list = await Client.GetFromJsonAsync<GetUsuariosResult>("/api/usuarios?pageNumber=1&pageSize=10");

        byIdResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        list.ShouldNotBeNull();
        list.TotalCount.ShouldBe(0);
    }

    [Test]
    public async Task Should_allow_reusing_email_after_soft_delete()
    {
        var created = await Context.SendAsync(new CreateUsuarioCommand("Ana", "Lopez", "reusable@example.com"));
        await Context.SendAsync(new DeleteUsuarioCommand(created.Id));

        var response = await Client.PostAsJsonAsync("/api/usuarios", new CreateUsuarioCommand("Bea", "Lopez", "reusable@example.com"));

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
    }
}