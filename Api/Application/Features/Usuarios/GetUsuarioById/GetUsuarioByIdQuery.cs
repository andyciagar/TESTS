using Mediator;

namespace Api.Application.Features.Usuarios.GetUsuarioById;

public sealed record GetUsuarioByIdQuery(Guid Id) : IRequest<GetUsuarioByIdResult?>;