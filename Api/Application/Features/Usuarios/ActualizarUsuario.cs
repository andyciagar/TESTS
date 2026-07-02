using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios;

public sealed record ActualizarUsuarioCommand(Guid Id, string Nombre, string Apellido, string Email) : IRequest<ActualizarUsuarioResult>;

public sealed record ActualizarUsuarioResult(Guid Id, string Nombre, string Apellido, string Email);

public sealed class ActualizarUsuarioCommandValidator : AbstractValidator<ActualizarUsuarioCommand>
{
    public ActualizarUsuarioCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("El id del usuario es requerido.");

        RuleFor(command => command.Nombre)
            .NotEmpty().WithMessage("El nombre es requerido.")
            .MaximumLength(100).WithMessage("El nombre no debe superar los 100 caracteres.");

        RuleFor(command => command.Apellido)
            .NotEmpty().WithMessage("El apellido es requerido.")
            .MaximumLength(100).WithMessage("El apellido no debe superar los 100 caracteres.");

        RuleFor(command => command.Email)
            .NotEmpty().WithMessage("El email es requerido.");
    }
}

public sealed class ActualizarUsuarioCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<ActualizarUsuarioCommand, ActualizarUsuarioResult>
{
    public async ValueTask<ActualizarUsuarioResult> Handle(ActualizarUsuarioCommand command, CancellationToken cancellationToken)
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

        return new ActualizarUsuarioResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}