using Mediator;
using Microsoft.EntityFrameworkCore;
using Api.Infrastructure.Data;

namespace Api.Application.Features.Usuarios.GetUsuarios;

public sealed class GetUsuariosHandler(ApplicationDbContext dbContext) : IRequestHandler<GetUsuariosQuery, GetUsuariosResult>
{
    public async ValueTask<GetUsuariosResult> Handle(GetUsuariosQuery query, CancellationToken cancellationToken)
    {
        var usuariosQuery = dbContext.Usuarios.AsNoTracking().OrderBy(usuario => usuario.Nombre).ThenBy(usuario => usuario.Apellido);
        var totalCount = await usuariosQuery.CountAsync(cancellationToken);

        var items = await usuariosQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(usuario => new GetUsuariosItemResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido))
            .ToListAsync(cancellationToken);

        return new GetUsuariosResult(query.PageNumber, query.PageSize, totalCount, items);
    }
}