using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpePorcentajesOcupacionAreasEstacionamiento.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.UpdateOpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.CreateOpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajeOcupacionAreaEstacionamientoById;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Commands.DeleteOpePorcentajesOcupacionAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePorcentsOcAE.Queries.GetOpePorcentajesOcupacionAreasEstacionamientoList;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-porcentajes-ocupacion-areas-estacionamiento")]
[ApiController]
public class OpePorcentajesOcupacionAreasEstacionamientoController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePorcentajesOcupacionAreasEstacionamientoController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePorcentajeOcupacionAreaEstacionamiento")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePorcentOcAEResponse>> Create([FromBody] CreateOpePorcentOcAECommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePorcentajeOcupacionAreaEstacionamiento>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePorcentajeOcupacionAreaEstacionamientoVm>>> GetOpePorcentajesOcupacionAreasEstacionamiento(
        [FromQuery] GetOpePorcentsOcAEListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de porcentaje de ocupación área de estacionamiento de OPE por id")]
    public async Task<ActionResult<OpePorcentajeOcupacionAreaEstacionamiento>> GetById(int id)
    {
        var query = new GetOpePorcentOcAEByIdQuery(id);
        var opePorcentajeOcupacionAreaEstacionamiento = await _mediator.Send(query);

        if (opePorcentajeOcupacionAreaEstacionamiento == null)
            return NotFound();

        return Ok(opePorcentajeOcupacionAreaEstacionamiento);
    }

    [HttpPut(Name = "UpdateOpePorcentajeOcupacionAreaEstacionamiento")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePorcentOcAECommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePorcentajeOcupacionAreaEstacionamiento")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePorcentOcAECommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
