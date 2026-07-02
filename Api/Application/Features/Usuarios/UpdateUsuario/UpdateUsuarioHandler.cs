using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios.UpdateUsuario;

public sealed class UpdateUsuarioHandler(ApplicationDbContext dbContext) : IRequestHandler<UpdateUsuarioCommand, UpdateUsuarioResult>
{
    public async ValueTask<UpdateUsuarioResult> Handle(UpdateUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuarioId = UsuarioId.From(command.Id);
        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(item => item.Id == usuarioId, cancellationToken)
            ?? throw new DomainException("El usuario indicado no existe.");

        var email = Email.From(command.Email.Trim().ToLowerInvariant());
        var emailExisteEnOtroUsuario = await dbContext.Usuarios
            .AnyAsync(item => item.Id != usuarioId && item.Email == email, cancellationToken);

        if (emailExisteEnOtroUsuario)
        {
            throw new DomainException("Ya existe un usuario con el email indicado.");
        }

        usuario.Actualizar(command.Nombre, command.Apellido, email);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateUsuarioResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}