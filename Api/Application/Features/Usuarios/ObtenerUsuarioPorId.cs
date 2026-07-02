using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios;

public sealed record ObtenerUsuarioPorIdQuery(Guid Id) : IRequest<ObtenerUsuarioPorIdResult?>;

public sealed record ObtenerUsuarioPorIdResult(Guid Id, string Nombre, string Apellido, string Email);

public sealed class ObtenerUsuarioPorIdQueryValidator : AbstractValidator<ObtenerUsuarioPorIdQuery>
{
    public ObtenerUsuarioPorIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty().WithMessage("El id del usuario es requerido.");
    }
}

public sealed class ObtenerUsuarioPorIdQueryHandler(ApplicationDbContext dbContext) : IRequestHandler<ObtenerUsuarioPorIdQuery, ObtenerUsuarioPorIdResult?>
{
    public async ValueTask<ObtenerUsuarioPorIdResult?> Handle(ObtenerUsuarioPorIdQuery query, CancellationToken cancellationToken)
    {
        var usuario = await dbContext.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == UsuarioId.From(query.Id), cancellationToken);

        return usuario is null
            ? null
            : new ObtenerUsuarioPorIdResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}