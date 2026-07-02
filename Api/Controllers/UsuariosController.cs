using Api.Application.Features.Usuarios;
using Api.Domain.Exceptions;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Vogen;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsuariosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<RegistrarUsuarioResult>> CreateAsync(
        [FromBody] RegistrarUsuarioCommand command,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(command, cancellationToken);
            return Created($"/api/usuarios/{result.Id}", result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
        catch (ValueObjectValidationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (DomainException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ObtenerUsuarioPorIdResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new ObtenerUsuarioPorIdQuery(id), cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
    }

    [HttpGet]
    public async Task<ActionResult<ObtenerUsuariosResult>> GetAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new ObtenerUsuariosQuery(pageNumber, pageSize), cancellationToken);
            return Ok(result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ActualizarUsuarioResult>> UpdateAsync(
        Guid id,
        [FromBody] ActualizarUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new ActualizarUsuarioCommand(id, request.Nombre, request.Apellido, request.Email), cancellationToken);
            return Ok(result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
        catch (ValueObjectValidationException exception)
        {
            return BadRequest(new { error = exception.Message });
        }
        catch (DomainException exception)
        {
            return NotFound(new { error = exception.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await mediator.Send(new EliminarUsuarioCommand(id), cancellationToken);
            return NoContent();
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
        catch (DomainException exception)
        {
            return NotFound(new { error = exception.Message });
        }
    }

    public sealed record ActualizarUsuarioRequest(string Nombre, string Apellido, string Email);
}