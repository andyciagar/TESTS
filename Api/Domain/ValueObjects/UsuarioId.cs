using Vogen;

namespace Api.Domain.ValueObjects;

[ValueObject<Guid>]
public readonly partial struct UsuarioId
{
    private static Validation Validate(Guid value)
        => value == Guid.Empty
            ? Validation.Invalid("El id del usuario es requerido.")
            : Validation.Ok;
}