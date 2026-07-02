using Mediator;

namespace Api.Application.Features.Usuarios.GetUsuarios;

public sealed record GetUsuariosQuery(int PageNumber = 1, int PageSize = 10) : IRequest<GetUsuariosResult>;