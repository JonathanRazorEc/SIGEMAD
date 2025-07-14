using DGPCE.Sigemad.Application.Dtos.OtraInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.DeleteOtraInformacionByRegistro;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Commands.ManageOtraInformaciones;
using DGPCE.Sigemad.Application.Features.OtrasInformaciones.Queries.GetOtraInformacion;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/otras-informaciones")]
[ApiController]
public class OtraInformacionController : ControllerBase
{
    private readonly IMediator _mediator;

    public OtraInformacionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<OtraInformacionDto>> GetotrasInformaciones(
    [FromQuery] int? idRegistroActualizacion,
    [FromQuery] int idSuceso)
    {
        var query = new GetOtraInformacionQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion,
            IdSuceso = idSuceso
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpDelete("{idRegistroActualizacion:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Eliminar un detalle de otra información mediante IdRegistroActualizacion")]
    public async Task<ActionResult> Delete(int idRegistroActualizacion)
    {
        var command = new DeleteOtraInformacionByIdRegistroCommand { IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpPost("lista")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista otra información")]
    public async Task<ActionResult<ManageOtraInformacionResponse>> CreateLista([FromBody] ManageOtraInformacionCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
