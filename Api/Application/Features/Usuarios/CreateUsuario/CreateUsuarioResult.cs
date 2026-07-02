namespace Api.Application.Features.Usuarios.CreateUsuario;

public sealed record CreateUsuarioResult(Guid Id, string Nombre, string Apellido, string Email);