namespace Api.FunctionalTests.Testing.Factories;

public static class UsuarioDataFactory
{
    public static RegistrarUsuarioCommand CreateCommand() => new Faker<RegistrarUsuarioCommand>()
        .CustomInstantiator(faker => new RegistrarUsuarioCommand(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Internet.Email(faker.Person.FirstName, faker.Person.LastName)));

    public static IReadOnlyCollection<RegistrarUsuarioCommand> CreateMany(int count)
        => Enumerable.Range(0, count)
            .Select(_ => CreateCommand())
            .ToArray();
}