using FluentValidation;

namespace Api.Application.Features.Usuarios.UpdateUsuario;

public sealed class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{
    public UpdateUsuarioCommandValidator()
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