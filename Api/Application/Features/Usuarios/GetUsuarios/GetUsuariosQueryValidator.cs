using FluentValidation;

namespace Api.Application.Features.Usuarios.GetUsuarios;

public sealed class GetUsuariosQueryValidator : AbstractValidator<GetUsuariosQuery>
{
    public GetUsuariosQueryValidator()
    {
        RuleFor(query => query.PageNumber)
            .GreaterThan(0).WithMessage("El numero de pagina debe ser mayor a cero.");

        RuleFor(query => query.PageSize)
            .GreaterThan(0).WithMessage("El tamano de pagina debe ser mayor a cero.")
            .LessThanOrEqualTo(100).WithMessage("El tamano de pagina no debe superar 100.");
    }
}