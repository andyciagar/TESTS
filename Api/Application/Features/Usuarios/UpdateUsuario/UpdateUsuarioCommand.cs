using Mediator;

namespace Api.Application.Features.Usuarios.UpdateUsuario;

public sealed record UpdateUsuarioCommand(Guid Id, string Nombre, string Apellido, string Email) : IRequest<UpdateUsuarioResult>;