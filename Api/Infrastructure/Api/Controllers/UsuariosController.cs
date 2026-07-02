using Api.Application.Features.Usuarios.DeleteUsuario;
using Api.Application.Features.Usuarios.CreateUsuario;
using Api.Application.Features.Usuarios.GetUsuarioById;
using Api.Application.Features.Usuarios.GetUsuarios;
using Api.Application.Features.Usuarios.UpdateUsuario;
using Api.Domain.Exceptions;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Vogen;

namespace Api.Infrastructure.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class UsuariosController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CreateUsuarioResult>> CreateAsync(
        [FromBody] CreateUsuarioCommand command,
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
    public async Task<ActionResult<GetUsuarioByIdResult>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new GetUsuarioByIdQuery(id), cancellationToken);
            return result is null ? NotFound() : Ok(result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
    }

    [HttpGet]
    public async Task<ActionResult<GetUsuariosResult>> GetAsync(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await mediator.Send(new GetUsuariosQuery(pageNumber, pageSize), cancellationToken);
            return Ok(result);
        }
        catch (ValidationException exception)
        {
            return BadRequest(new { errors = exception.Errors.Select(error => error.ErrorMessage) });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<UpdateUsuarioResult>> UpdateAsync(
        Guid id,
        [FromBody] UpdateUsuarioRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await mediator.Send(new UpdateUsuarioCommand(id, request.Nombre, request.Apellido, request.Email), cancellationToken);
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
            await mediator.Send(new DeleteUsuarioCommand(id), cancellationToken);
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

    public sealed record UpdateUsuarioRequest(string Nombre, string Apellido, string Email);
}