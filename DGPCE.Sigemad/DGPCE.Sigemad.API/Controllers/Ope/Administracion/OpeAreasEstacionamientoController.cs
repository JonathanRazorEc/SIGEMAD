using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.DeleteOpeAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreaEstacionamientoById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.CreateOpeAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Commands.UpdateOpeAreasEstacionamiento;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Queries.GetOpeAreasEstacionamientoList;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasEstacionamiento.Vms;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-areas-estacionamiento")]
[ApiController]
public class OpeAreasEstacionamientoController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAreasEstacionamientoController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeAreaEstacionamiento")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeAreaEstacionamientoResponse>> Create([FromBody] CreateOpeAreaEstacionamientoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeAreaEstacionamiento>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeAreaEstacionamientoVm>>> GetOpeAreasEstacionamiento(
        [FromQuery] GetOpeAreasEstacionamientoListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de área de estacionamiento de OPE por id")]
    public async Task<ActionResult<OpeAreaEstacionamiento>> GetById(int id)
    {
        var query = new GetOpeAreaEstacionamientoByIdQuery(id);
        var opeAreaEstacionamiento = await _mediator.Send(query);

        if (opeAreaEstacionamiento == null)
            return NotFound();

        return Ok(opeAreaEstacionamiento);
    }

    [HttpPut(Name = "UpdateOpeAreaEstacionamiento")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeAreaEstacionamientoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeAreaEstacionamiento")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeAreaEstacionamientoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
