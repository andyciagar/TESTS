namespace Api.FunctionalTests.Testing.Factories;

public static class UsuarioDataFactory
{
    public static CreateUsuarioCommand CreateCommand() => new Faker<CreateUsuarioCommand>()
        .CustomInstantiator(faker => new CreateUsuarioCommand(
            faker.Person.FirstName,
            faker.Person.LastName,
            faker.Internet.Email(faker.Person.FirstName, faker.Person.LastName)));

    public static IReadOnlyCollection<CreateUsuarioCommand> CreateMany(int count)
        => Enumerable.Range(0, count)
            .Select(_ => CreateCommand())
            .ToArray();
}