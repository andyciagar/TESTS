using FluentValidation;

namespace Api.Application.Features.Usuarios.DeleteUsuario;

public sealed class DeleteUsuarioCommandValidator : AbstractValidator<DeleteUsuarioCommand>
{
    public DeleteUsuarioCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("El id del usuario es requerido.");
    }
}