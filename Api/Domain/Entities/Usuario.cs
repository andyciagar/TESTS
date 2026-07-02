using Api.Domain.Common;
using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public sealed class Usuario : Entity<Guid>
{
    public string Nombre { get; private set; }
    public string Apellido { get; private set; }
    public Email Email { get; private set; }

    public Usuario(Guid id, string nombre, string apellido, Email email)
        : base(id)
    {
        if (id == Guid.Empty)
        {
            throw new DomainException("El id del usuario es requerido.");
        }

        Nombre = ValidarTexto(nombre, "nombre");
        Apellido = ValidarTexto(apellido, "apellido");
        Email = email;
    }

    private static string ValidarTexto(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"El {fieldName} es requerido.");
        }

        return value.Trim();
    }
}