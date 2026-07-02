using Api.Domain.Common;
using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;

namespace Api.Domain.Entities;

public sealed class Usuario : Entity<UsuarioId>
{
    public string Nombre { get; private set; }
    public string Apellido { get; private set; }
    public Email Email { get; private set; }
    public bool IsDeleted { get; private set; }

    public Usuario(UsuarioId id, string nombre, string apellido, Email email)
        : base(id)
    {
        Nombre = ValidarTexto(nombre, "nombre");
        Apellido = ValidarTexto(apellido, "apellido");
        Email = email;
    }

    public void Actualizar(string nombre, string apellido, Email email)
    {
        Nombre = ValidarTexto(nombre, "nombre");
        Apellido = ValidarTexto(apellido, "apellido");
        Email = email;
    }

    public void Eliminar()
    {
        IsDeleted = true;
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