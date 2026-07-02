namespace Api.Application.Features.Usuarios.GetUsuarios;

public sealed record GetUsuariosResult(int PageNumber, int PageSize, int TotalCount, IReadOnlyCollection<GetUsuariosItemResult> Items);