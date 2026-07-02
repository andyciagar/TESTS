using Api.FunctionalTests.Testing;

namespace Api.FunctionalTests.Features.Usuarios.Commands;

public sealed class RegistrarUsuarioEdgeCasesTests : FunctionalTestBase
{
    [TestCase("", "Perez", "correo@example.com")]
    [TestCase("Juan", "", "correo@example.com")]
    [TestCase("Juan", "Perez", "")]
    public async Task Should_return_bad_request_when_required_fields_are_missing(string nombre, string apellido, string email)
    {
        var command = new CreateUsuarioCommand(nombre, apellido, email);

        var response = await Client.PostAsJsonAsync("/api/usuarios", command);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_return_bad_request_when_nombre_exceeds_max_length()
    {
        var command = new CreateUsuarioCommand(new string('A', 101), "Perez", "correo@example.com");

        var response = await Client.PostAsJsonAsync("/api/usuarios", command);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_return_bad_request_when_email_is_invalid()
    {
        var command = new CreateUsuarioCommand("Juan", "Perez", "correo-invalido");

        var response = await Client.PostAsJsonAsync("/api/usuarios", command);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Test]
    public async Task Should_not_allow_duplicate_email_for_active_users()
    {
        var first = new CreateUsuarioCommand("Juan", "Perez", "duplicado@example.com");
        var second = new CreateUsuarioCommand("Ana", "Gomez", "duplicado@example.com");

        await Context.SendAsync(first);

        var response = await Client.PostAsJsonAsync("/api/usuarios", second);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}