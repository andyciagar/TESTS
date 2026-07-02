using Api.Domain.Entities;
using Api.Domain.Exceptions;
using Api.Domain.ValueObjects;
using Api.Infrastructure.Data;
using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace Api.Application.Features.Usuarios;

public sealed record RegistrarUsuarioCommand(string Nombre, string Apellido, string Email) : IRequest<RegistrarUsuarioResult>;

public sealed record RegistrarUsuarioResult(Guid Id, string Nombre, string Apellido, string Email);

public sealed class RegistrarUsuarioCommandValidator : AbstractValidator<RegistrarUsuarioCommand>
{
    public RegistrarUsuarioCommandValidator()
    {
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

public sealed class RegistrarUsuarioCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<RegistrarUsuarioCommand, RegistrarUsuarioResult>
{
    public async ValueTask<RegistrarUsuarioResult> Handle(RegistrarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var email = Email.From(command.Email.Trim().ToLowerInvariant());

        if (await dbContext.Usuarios.AnyAsync(usuario => usuario.Email == email, cancellationToken))
        {
            throw new DomainException("Ya existe un usuario con el email indicado.");
        }

        var usuario = new Usuario(UsuarioId.From(Guid.NewGuid()), command.Nombre, command.Apellido, email);

        dbContext.Usuarios.Add(usuario);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegistrarUsuarioResult(usuario.Id.Value, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}