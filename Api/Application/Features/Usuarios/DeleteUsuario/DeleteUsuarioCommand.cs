using Mediator;

namespace Api.Application.Features.Usuarios.DeleteUsuario;

public sealed record DeleteUsuarioCommand(Guid Id) : IRequest<Unit>;