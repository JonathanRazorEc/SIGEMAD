using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Vms;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Queries.GetOpePuntosControlCarreterasList;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.CreateOpePuntosControlCarreteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.UpdateOpePuntosControlCarreteras;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Queries.GetOpePuntoControlCarreteraById;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpePuntosControlCarreteras.Commands.DeleteOpePuntosControlCarreteras;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-puntos-control-carreteras")]
[ApiController]
public class OpePuntosControlCarreterasController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpePuntosControlCarreterasController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpePuntoControlCarretera")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpePuntoControlCarreteraResponse>> Create([FromBody] CreateOpePuntoControlCarreteraCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpePuntoControlCarretera>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpePuntoControlCarreteraVm>>> GetOpePuntosControlCarreteras(
        [FromQuery] GetOpePuntosControlCarreterasListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de punto de control de carretera de OPE por id")]
    public async Task<ActionResult<OpePuntoControlCarretera>> GetById(int id)
    {
        var query = new GetOpePuntoControlCarreteraByIdQuery(id);
        var opePuntoControlCarretera = await _mediator.Send(query);

        if (opePuntoControlCarretera == null)
            return NotFound();

        return Ok(opePuntoControlCarretera);
    }

    [HttpPut(Name = "UpdateOpePuntoControlCarretera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpePuntoControlCarreteraCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpePuntoControlCarretera")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpePuntoControlCarreteraCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
