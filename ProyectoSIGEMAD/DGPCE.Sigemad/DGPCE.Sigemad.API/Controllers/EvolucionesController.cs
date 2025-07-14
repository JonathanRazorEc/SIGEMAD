using DGPCE.Sigemad.Application.Dtos.Evoluciones;
using DGPCE.Sigemad.Application.Dtos.IntervencionMedios;
using DGPCE.Sigemad.Application.Features.Evoluciones.Commands.DeleteEvolucionesByIdRegistro;
using DGPCE.Sigemad.Application.Features.Evoluciones.Queries.GetEvolucion;
using DGPCE.Sigemad.Application.Features.ImpactosEvoluciones.Commands.CreateListaImpactoEvolucion;
using DGPCE.Sigemad.Application.Features.IntervencionesMedios.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DGPCE.Sigemad.API.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class EvolucionesController : ControllerBase
{
    private readonly IMediator _mediator;

    public EvolucionesController(IMediator mediator)
    {
        _mediator = mediator;

    }

    //[HttpGet]
    //public async Task<ActionResult<EvolucionDto>> GetEvolucion(
    //    [FromQuery] int? idRegistroActualizacion,
    //    [FromQuery] int idSuceso)
    //{
    //    var query = new GetEvolucionQuery
    //    {
    //        IdRegistroActualizacion = idRegistroActualizacion,
    //        IdSuceso = idSuceso
    //    };
    //    var result = await _mediator.Send(query);
    //    return Ok(result);
    //}
    


    //[HttpPost("intervenciones")]
    //[ProducesResponseType(StatusCodes.Status201Created)]
    //[ProducesResponseType(StatusCodes.Status400BadRequest)]
    //[SwaggerOperation(Summary = "Crear lista de intervenciones de una evolucion")]
    //public async Task<ActionResult<ManageIntervencionMedioResponse>> CreateIntervenciones([FromBody] ManageIntervencionMedioCommand command)
    //{
    //    var response = await _mediator.Send(command);
    //    return Ok(response);
    //}

    /*
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Eliminar evolución por id")]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteEvolucionCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
    */

    /*
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Eliminar evolución por id")]
    public async Task<ActionResult> Delete([FromQuery] DeleteEvolucionByIdRegistroCommand command)
    {
        //var command = new DeleteEvolucionCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
    */

    //[HttpDelete("{idRegistroActualizacion}")]
    //[ProducesResponseType(StatusCodes.Status204NoContent)]
    //[ProducesResponseType(StatusCodes.Status404NotFound)]
    //[SwaggerOperation(Summary = "Eliminar evolución por id Registro de actualizacion")]
    //public async Task<ActionResult> Delete(int idRegistroActualizacion)
    //{
    //    var command = new DeleteEvolucionByIdRegistroCommand { IdRegistroActualizacion = idRegistroActualizacion };
    //    await _mediator.Send(command);
    //    return NoContent();
    //}

}
