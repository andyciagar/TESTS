namespace Api.Application.Features.Usuarios.GetUsuarioById;

public sealed record GetUsuarioByIdResult(Guid Id, string Nombre, string Apellido, string Email);