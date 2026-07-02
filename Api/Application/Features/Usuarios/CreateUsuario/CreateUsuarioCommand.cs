using Mediator;

namespace Api.Application.Features.Usuarios.CreateUsuario;

public sealed record CreateUsuarioCommand(string Nombre, string Apellido, string Email) : IRequest<CreateUsuarioResult>;