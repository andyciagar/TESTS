using Api.Domain.Entities;
using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios.CreateUsuario;

public sealed class CreateUsuarioHandler(ApplicationDbContext dbContext) : IRequestHandler<CreateUsuarioCommand, CreateUsuarioResult>
{
    public async ValueTask<CreateUsuarioResult> Handle(CreateUsuarioCommand command, CancellationToken cancellationToken)
    {
        var email = Email.From(command.Email.Trim().ToLowerInvariant());

        if (await dbContext.Usuarios.AnyAsync(usuario => usuario.Email == email, cancellationToken))
        {
            throw new DomainException("Ya existe un usuario con el email indicado.");
        }

        var usuario = new Usuario(UsuarioId.From(Guid.NewGuid()), command.Nombre, command.Apellido, email);

        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new CreateUsuarioResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}