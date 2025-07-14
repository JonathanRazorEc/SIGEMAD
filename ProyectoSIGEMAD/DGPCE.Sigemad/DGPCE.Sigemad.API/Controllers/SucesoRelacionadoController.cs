using Azure.Core;
using DGPCE.Sigemad.Application.Dtos.SucesoRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.DeleteSucesoRelacionadoPorRegistro;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Commands.ManageSucesoRelacionados;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Queries.GetSucesoRelacionado;
using DGPCE.Sigemad.Application.Features.SucesosRelacionados.Vms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/sucesos-relacionados")]
[ApiController]
public class SucesoRelacionadoController : ControllerBase
{
    private readonly IMediator _mediator;

    public SucesoRelacionadoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Obtiene un suceso relacionado por su id")]
    public async Task<ActionResult<SucesoRelacionadoVm>> GetSucesoRelacionado([FromQuery] int? idRegistroActualizacion,
    [FromQuery] int idSuceso)
    {
        var query = new GetSucesoRelacionadoQuery
        {
            IdRegistroActualizacion = idRegistroActualizacion,
            IdSuceso = idSuceso
        };
        var sucesoRelacionado = await _mediator.Send(query);
        return Ok(sucesoRelacionado);
    }

    [HttpPost("lista")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Summary = "Crear lista de sucesos relacionados")]
    public async Task<ActionResult<ManageSucesoRelacionadoResponse>> CreateLista([FromBody] ManageSucesoRelacionadosCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete("{idRegistroActualizacion:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [SwaggerOperation(Summary = "Eliminar un suceso relacionado por su id")]
    public async Task<ActionResult> DeleteSucesoRelacionado(int idRegistroActualizacion)
    {
        //var command = new DeleteSucesosRelacionadosCommand { Id = id };
        var command = new DeleteSucesoRelacionadoPorRegistroCommand { IdRegistroActualizacion = idRegistroActualizacion };
        await _mediator.Send(command);
        return NoContent();
    }

}
