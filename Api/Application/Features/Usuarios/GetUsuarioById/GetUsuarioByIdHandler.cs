using Api.Application.Abstractions;

namespace Api.Application.Features.Usuarios.GetUsuarioById;

public sealed class GetUsuarioByIdHandler(IUsuarioRepository usuarioRepository)
{
    public async Task<GetUsuarioByIdResult?> HandleAsync(GetUsuarioByIdQuery query, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.GetByIdAsync(query.Id, cancellationToken);

        return usuario is null
            ? null
            : new GetUsuarioByIdResult(usuario.Id, usuario.Nombre, usuario.Apellido, usuario.Email.Value);
    }
}