namespace Api.Application.Features.Usuarios.UpdateUsuario;

public sealed record UpdateUsuarioResult(Guid Id, string Nombre, string Apellido, string Email);