using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios.GetUsuarioById;

public sealed class GetUsuarioByIdHandler(ApplicationDbContext dbContext) : IRequestHandler<GetUsuarioByIdQuery, GetUsuarioByIdResult?>
{
    public async ValueTask<GetUsuarioByIdResult?> Handle(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
    {
        var usuario = await dbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == UsuarioId.From(query.Id), cancellationToken);

        return usuario is null
            ? null
            : new GetUsuarioByIdResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}