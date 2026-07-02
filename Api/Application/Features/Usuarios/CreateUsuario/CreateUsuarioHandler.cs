using Api.Application.Abstractions;
using Api.Domain.Entities;
using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;

namespace Api.Application.Features.Usuarios.CreateUsuario;

public sealed class CreateUsuarioHandler(IUsuarioRepository usuarioRepository)
{
    public async Task<CreateUsuarioResult> HandleAsync(CreateUsuarioCommand command, CancellationToken cancellationToken)
    {
        var email = Email.From(command.Email.Trim().ToLowerInvariant());

        if (await usuarioRepository.ExistsByEmailAsync(email, cancellationToken))
        {
            throw new DomainException("Ya existe un usuario con el email indicado.");
        }

        var usuario = new Usuario(Guid.NewGuid(), command.Nombre, command.Apellido, email);

        await usuarioRepository.AddAsync(usuario, cancellationToken);

        return new CreateUsuarioResult(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}