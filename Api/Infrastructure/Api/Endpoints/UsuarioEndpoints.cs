using Api.Application.Features.Usuarios.CreateUsuario;
using Api.Application.Features.Usuarios.GetUsuarioById;
using Api.Domain.Exceptions;
using Vogen;

namespace Api.Infrastructure.Api.Endpoints;

public static class UsuarioEndpoints
{
    public static IEndpointRouteBuilder MapUsuarioEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/usuarios")
            .WithTags("Usuarios");

        group.MapPost("/", CreateAsync)
            .WithName("CreateUsuario");

        group.MapGet("/{id:guid}", GetByIdAsync)
            .WithName("GetUsuarioById");

        return app;
    }

    private static async Task<IResult> CreateAsync(
        CreateUsuarioRequest request,
        CreateUsuarioHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.HandleAsync(
                new CreateUsuarioCommand(request.Nombre, request.Apellido, request.Email),
                cancellationToken);

            return Results.Created($"/usuarios/{result.Id}", result);
        }
        catch (DomainException exception)
        {
            return Results.BadRequest(new { error = exception.Message });
        }
        catch (ValueObjectValidationException exception)
        {
            return Results.BadRequest(new { error = exception.Message });
        }
    }

    private static async Task<IResult> GetByIdAsync(
        Guid id,
        GetUsuarioByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new GetUsuarioByIdQuery(id), cancellationToken);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private sealed record CreateUsuarioRequest(string Nombre, string Apellido, string Email);
}