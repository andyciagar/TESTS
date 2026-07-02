using Api.FunctionalTests.Testing;
using Api.FunctionalTests.Testing.Factories;

namespace Api.FunctionalTests.Features.Usuarios;

public sealed class ObtenerUsuariosTests : FunctionalTestBase
{
    [Test]
    public async Task Should_get_paginated_usuarios_without_email_in_list()
    {
        var commands = UsuarioDataFactory.CreateMany(3);

        foreach (var command in commands)
        {
            await Context.SendAsync(command);
        }

        var result = await Client.GetFromJsonAsync<GetUsuariosResult>("/api/usuarios?pageNumber=1&pageSize=2");

        result.ShouldNotBeNull();
        result.PageNumber.ShouldBe(1);
        result.PageSize.ShouldBe(2);
        result.TotalCount.ShouldBe(3);
        result.Items.Count.ShouldBe(2);
        result.Items.First().GetType().GetProperty("Email").ShouldBeNull();
    }
}