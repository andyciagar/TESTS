using Api.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios;

public sealed record ObtenerUsuariosQuery(int PageNumber = 1, int PageSize = 10) : IRequest<ObtenerUsuariosResult>;

public sealed record ObtenerUsuariosItemResult(Guid Id, string Nombre, string Apellido);

public sealed record ObtenerUsuariosResult(int PageNumber, int PageSize, int TotalCount, IReadOnlyCollection<ObtenerUsuariosItemResult> Items);

public sealed class ObtenerUsuariosQueryValidator : AbstractValidator<ObtenerUsuariosQuery>
{
    public ObtenerUsuariosQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0).WithMessage("El numero de pagina debe ser mayor a cero.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("El tamano de pagina debe ser mayor a cero.")
            .LessThanOrEqualTo(100).WithMessage("El tamano de pagina no debe superar 100.");
    }
}

public sealed class ObtenerUsuariosQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<ObtenerUsuariosQuery, ObtenerUsuariosResult>
{
    public async ValueTask<ObtenerUsuariosResult> Handle(ObtenerUsuariosQuery query, CancellationToken cancellationToken)
    {
        var usuariosQuery = dbContext.Usuarios.AsNoTracking().OrderBy(usuario => usuario.Nombre).ThenBy(usuario => usuario.Apellido);
        var totalCount = await usuariosQuery.CountAsync(cancellationToken);

        var items = await usuariosQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(usuario => new ObtenerUsuariosItemResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido))
            .ToListAsync(cancellationToken);

        return new ObtenerUsuariosResult(query.PageNumber, query.PageSize, totalCount, items);
    }
}