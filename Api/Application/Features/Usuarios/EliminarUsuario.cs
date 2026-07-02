using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios;

public sealed record EliminarUsuarioCommand(Guid Id) : IRequest<Unit>;

public sealed class EliminarUsuarioCommandValidator : AbstractValidator<EliminarUsuarioCommand>
{
    public EliminarUsuarioCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("El id del usuario es requerido.");
    }
}

public sealed class EliminarUsuarioCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<EliminarUsuarioCommand>
{
    public async ValueTask<Unit> Handle(EliminarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuarioId = UsuarioId.From(command.Id);
        var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(item => item.Id == usuarioId, cancellationToken)
            ?? throw new DomainException("El usuario indicado no existe.");

        usuario.Eliminar();
        await dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}