using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios.DeleteUsuario;

public sealed class DeleteUsuarioHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteUsuarioCommand>
{
    public async ValueTask<Unit> Handle(DeleteUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuarioId = UsuarioId.From(command.Id);
        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(item => item.Id == usuarioId, cancellationToken)
            ?? throw new DomainException("El usuario indicado no existe.");

        usuario.Eliminar();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}