using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.DeleteOpeAreasDescanso;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreaDescansoById;
using DGPCE.Sigemad.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using DGPCE.Sigemad.Application.Features.Ope.Datos.OpeAreasDescanso.Vms;
using DGPCE.Sigemad.Domain.Modelos.Ope.Administracion;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.CreateOpeAreasDescanso;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Commands.UpdateOpeAreasDescanso;
using DGPCE.Sigemad.Application.Features.Ope.Administracion.OpeAreasDescanso.Queries.GetOpeAreasDescansoList;

namespace DGPCE.Sigemad.API.Controllers.Ope.Administracion;

[Authorize]
[Route("api/v1/ope-areas-descanso")]
[ApiController]
public class OpeAreasDescansoController : ControllerBase
{
    private readonly IMediator _mediator;

    public OpeAreasDescansoController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpPost(Name = "CreateOpeAreaDescanso")]
    [ProducesResponseType((int)HttpStatusCode.Created)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<ActionResult<CreateOpeAreaDescansoResponse>> Create([FromBody] CreateOpeAreaDescansoCommand command)
    {
        var response = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PaginationVm<OpeAreaDescanso>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<OpeAreaDescansoVm>>> GetOpeAreasDescanso(
        [FromQuery] GetOpeAreasDescansoListQuery query)
    {
        var pagination = await _mediator.Send(query);
        return Ok(pagination);
    }


    [HttpGet("{id}")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [SwaggerOperation(Summary = "Busqueda de área de descanso de OPE por id")]
    public async Task<ActionResult<OpeAreaDescanso>> GetById(int id)
    {
        var query = new GetOpeAreaDescansoByIdQuery(id);
        var opeAreaDescanso = await _mediator.Send(query);

        if (opeAreaDescanso == null)
            return NotFound();

        return Ok(opeAreaDescanso);
    }

    [HttpPut(Name = "UpdateOpeAreaDescanso")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Update([FromBody] UpdateOpeAreaDescansoCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteOpeAreaDescanso")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(int id)
    {
        var command = new DeleteOpeAreaDescansoCommand { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
