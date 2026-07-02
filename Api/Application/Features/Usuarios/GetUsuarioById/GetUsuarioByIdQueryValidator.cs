using FluentValidation;

namespace Api.Application.Features.Usuarios.GetUsuarioById;

public sealed class GetUsuarioByIdQueryValidator : AbstractValidator<GetUsuarioByIdQuery>
{
    public GetUsuarioByIdQueryValidator()
    {
        RuleFor(query => query.Id)
            .NotEmpty().WithMessage("El id del usuario es requerido.");
    }
}